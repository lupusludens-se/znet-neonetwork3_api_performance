using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Caching.Distributed;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Data;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.EmailAlert;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace SE.Neo.WebAPI.Services
{
    public class UserApiService : IUserApiService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly ICommonService _commonService;
        private readonly INotificationService _notificationService;
        private readonly IDistributedCache _cache;
        private readonly IProjectService _projectService;
        private readonly IEmailAlertService _emailAlertService;

        public UserApiService(ApplicationContext context, ILogger<UserApiService> logger, IMapper mapper,
            IUserService userService, ICommonService commonService, INotificationService notificationService,
            IDistributedCache cache, IProjectService projectService, IEmailAlertService emailAlertService)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userService = userService;
            _commonService = commonService;
            _notificationService = notificationService;
            _cache = cache;
            _projectService = projectService;
            _emailAlertService = emailAlertService;
        }

        public async Task<WrapperModel<UserResponse>> GetUsersAsync(BaseSearchFilterModel filter, UserModel userDetails, bool isOwnCompanyUsersRequest = false, bool accessPrivateInfo = false)
        {
            WrapperModel<UserDTO> usersResult = await _userService.GetUsersAsync(filter, userDetails.Id, userDetails.RoleIds, userDetails.CompanyId, isOwnCompanyUsersRequest, accessPrivateInfo);

            var wrapper = new WrapperModel<UserResponse>
            {
                Count = usersResult.Count,
                DataList = usersResult.DataList.Select(_mapper.Map<UserResponse>)
            };
            return wrapper;
        }

        public async Task<int> ExportUsersAsync(BaseSearchFilterModel filter, MemoryStream stream, UserModel userDetails, bool isSPAdminRequest = false)
        {
            filter.Expand = "company,userprofile.state,userprofile.country,userprofile.regions,roles";
            WrapperModel<UserDTO> usersResult = await _userService.GetUsersAsync(filter, userDetails.Id, userDetails.RoleIds, userDetails.CompanyId, isSPAdminRequest);

            using (var writeFile = new StreamWriter(stream, leaveOpen: true))
            {
                using (var csv = new CsvWriter(writeFile, new CsvConfiguration(CultureInfo.InvariantCulture) { Encoding = Encoding.UTF8, LeaveOpen = true }))
                {
                    var classMap = new DefaultClassMap<UserExportResponse>();
                    classMap.Map(o => o.FirstName).Name("First Name").Index(0);
                    classMap.Map(o => o.LastName).Name("Last Name").Index(1);
                    classMap.Map(o => o.Email).Name("Email").Index(2);

                    if (userDetails.RoleIds.Contains((int)RoleType.SPAdmin))
                    {
                        classMap.Map(o => o.Status).Name("Status").Index(3);
                        classMap.Map(o => o.Country).Name("Country").Index(4);
                        classMap.Map(o => o.State).Name("State").Index(5);
                    }
                    else
                    {
                        classMap.Map(o => o.Company).Name("Company").Index(3);
                        classMap.Map(o => o.Roles).Name("Roles").Index(4);

                        classMap.Map(o => o.Status).Name("Status").Index(5);
                        classMap.Map(o => o.Country).Name("Country").Index(6);
                        classMap.Map(o => o.State).Name("State").Index(7);
                        classMap.Map(o => o.Regions).Name("Regions").Index(8);
                        classMap.Map(o => o.ApprovedBy).Name("Approved By").Index(9);
                    }

                    csv.Context.RegisterClassMap(classMap);
                    csv.WriteRecords(usersResult.DataList.Select(_mapper.Map<UserExportResponse>));
                }
            }
            stream.Position = 0;

            return usersResult.Count;
        }

        public async Task<UserResponse?> GetUserAsync(int id, string? expand, bool accessPrivateInfo = false)
        {
            var modelDTO = await _userService.GetUserAsync(id, expand, accessPrivateInfo);
            var userResp = _mapper.Map<UserResponse>(modelDTO);
            if (expand != null && expand.Contains("emailAlerts"))
            {
                var emailalerts = await _emailAlertService.GetUserEmailAlertsAsync(id);
                userResp.EmailAlerts = emailalerts.DataList.Select(x => _mapper.Map<EmailAlertResponse>(x)).ToList();
            }

            return userResp;
        }

        public async Task<UserResponse?> GetSPAdminByCompany(int companyId, int? userId)
        {
            var modelDTO = await _userService.GetSPAdminByCompany(companyId, userId);
            return _mapper.Map<UserResponse>(modelDTO);
        }

        public async Task<int> CreateUpdateUserAsync(int id, UserRequest model)
        {
            var emailAlerts = model?.EmailAlerts?.EmailAlertsData.Select(x => _mapper.Map<EmailAlertDTO>(x));
            var modelDTO = _mapper.Map<UserDTO>(model);
            if (modelDTO.TimeZoneId == 0)
            {
                modelDTO.TimeZoneId = (await _commonService.GetTimeZoneByClientIdOrDefault()).Id;
            }

            int userId = await _userService.CreateUpdateUserAsync(id, modelDTO, emailAlerts);

            if (id != 0)
            {
                _cache.Remove(modelDTO.Username);
            }

            return userId;
        }

        public async Task<bool> PatchUserAsync(int id, JsonPatchDocument patchDoc, UserModel userDetails, bool isAdminRequest = false)
        {
            try
            {
                UserDTO? currentUserDTO = await _userService.GetUserAsync(id);

                if (userDetails.RoleIds.Contains((int)RoleType.SPAdmin) && userDetails.CompanyId == currentUserDTO?.CompanyId || userDetails.RoleIds.Contains((int)RoleType.Admin) || userDetails.Id == currentUserDTO?.Id)
                {
                    UserDTO? userDTO = await _userService.PatchUserAsync(id, patchDoc);
                    if (userDTO != null)
                    {
                        _cache.Remove(userDTO.Username);

                        if (!isAdminRequest && currentUserDTO!.StatusId != userDTO.StatusId && userDTO.StatusId == (int)UserStatus.Deleted)
                        {
                            var adminIds = await _userService.GetAdminUsersIdsAsync();
                            await _notificationService.AddNotificationsAsync(adminIds, NotificationType.UserDeleted,
                                new UserDeletedNotificationDetails
                                {
                                    UserId = userDTO.Id,
                                    UserName = $"{userDTO.FirstName} {userDTO.LastName}"
                                });
                        }
                    }
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorUpdatingUser);
                return false;
            }
        }

        public async Task<UserModel?> GetUserModelByUsernameAsync(string username)
        {
            _logger.LogInformation(string.Format("Getting User by Username {0}.", username));

            var user = await _userService.GetUserByUsernameAsync(username);

            if (user == null)
                return null;

            return _mapper.Map<UserModel>(user);
        }

        public async Task CreateUserFollowerAsync(int followerId, int followedId)
        {
            await _userService.CreateUserFollowerAsync(followerId, followedId);

            // notifications
            try
            {
                UserDTO? userDTO = await _userService.GetUserAsync(followerId);
                if (userDTO != null)
                {
                    await _notificationService.AddNotificationAsync(followedId, NotificationType.FollowsMe,
                        new FollowerNotificationDetails
                        {
                            FollowerName = $"{userDTO.FirstName} {userDTO.LastName}",
                            FollowerUserId = followerId
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }
        }

        public async Task RemoveUserFollowerAsync(int followerId, int followedId)
        {
            await _userService.RemoveUserFollowerAsync(followerId, followedId);
        }

        public bool IsInternalCorporationUser(int userId)
        {
            return _userService.IsInternalCorporationUser(userId);
        }

        public async Task<bool> DeleteUserAsync(int id, UserModel userDetails)
        {
            UserDTO? resultUserDTO = await _userService.GetUserAsync(id);

            if (userDetails != null && resultUserDTO != null && userDetails.RoleIds.Contains((int)RoleType.SPAdmin) && userDetails.CompanyId == resultUserDTO?.CompanyId || 
                (userDetails.RoleIds.Contains((int)RoleType.Admin) && !userDetails.RoleIds.Contains((int)RoleType.SystemOwner) && resultUserDTO.Roles.Select(x => x.Id).Contains((int)RoleType.SystemOwner)) 
                || userDetails.RoleIds.Contains((int)RoleType.SystemOwner))
            {
                await _userService.DeleteUserAsync(resultUserDTO.Id);
                return true;
            }
            else
                return false;
        }

        public async Task RequestToDeleteUserAsync(int id, string userName)
        {
            _cache.Remove(userName);
            try
            {
                await _userService.RequestToDeleteUserAsync(id, userName);
            }
            catch (Exception ex)
            {
                // If is an error with sending email we are logging that, and processing request;
                _logger.LogError(ex, ex.Message);
            }
        }

        public bool IsNewUser(int userId)
        {
            return _projectService.IsNewUser(userId);
        }

        public async Task<bool> UpdateOnboardUserStatus()
        {
            try
            {
                return await _userService.UpdateOnboardUserStatus();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Get Category and Skills for both Corporate and SP User
        /// </summary>
        /// <param name="user">Contains current user</param>
        /// <returns>Categories and Skills for SP Users, contains Skills for Corporate users</returns>
        public async Task<List<SkillsByCategoryResponse>> GetSkillsByCategory(UserModel user)
        {
            try
            {
                List<SkillsByCategoryDTO> modelDTO = await _userService.GetSkillsByCategory(user.RoleIds, user.Id);
                var response = _mapper.Map<List<SkillsByCategoryResponse>>(modelDTO);
                return response;
            }
            catch (Exception ex) { throw; }
        }
    }
}
