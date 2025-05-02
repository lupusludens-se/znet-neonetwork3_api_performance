using AutoMapper;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Net.Mail;

namespace SE.Neo.WebAPI.Services
{
    public class UserPendingApiService : IUserPendingApiService
    {
        private readonly ILogger<UserPendingApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserPendingService _userPendingService;
        private readonly ICompanyService _companyService;
        private readonly ICommonService _commonService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public UserPendingApiService(
            ILogger<UserPendingApiService> logger,
            IMapper mapper,
            IUserPendingService userPendingService,
            ICompanyService companyService,
            ICommonService commonService,
            INotificationService notificationService,
            IUserService userService
            )
        {
            _logger = logger;
            _mapper = mapper;
            _userPendingService = userPendingService;
            _companyService = companyService;
            _commonService = commonService;
            _notificationService = notificationService;
            _userService = userService;
        }

        public async Task<WrapperModel<UserPendingListItemResponse>> GetUserPendingsAsync(ExpandOrderModel filter)
        {
            WrapperModel<UserPendingDTO> userPendingsResult = await _userPendingService.GetUserPendingsAsync(filter);
            var wrapper = new WrapperModel<UserPendingListItemResponse>
            {
                Count = userPendingsResult.Count,
                DataList = userPendingsResult.DataList.Select(_mapper.Map<UserPendingListItemResponse>)
            };
            return wrapper;
        }

        public async Task<UserPendingItemResponse?> GetUserPendingAsync(int id, string? expand)
        {
            UserPendingDTO? modelDTO = await _userPendingService.GetUserPendingAsync(id, expand);
            return _mapper.Map<UserPendingItemResponse>(modelDTO);
        }

        public async Task<int> CreateUpdateUserPendingAsync(UserPendingRequest model, int id = 0)
        {
            var modelDTO = _mapper.Map<UserPendingDTO>(model);
            modelDTO.Id = id;
            CompanyDTO company = null;
            UserDTO user = null;
            if (model is UserPendingAddRequest)
            {
                var addModel = (model as UserPendingAddRequest)!;
                company = await _companyService.GetCompanyByName(addModel.CompanyName);
                if (company != null)
                {
                    modelDTO.CompanyId = company.Id;
                }

                modelDTO.TimeZoneId = (await _commonService.GetTimeZoneByClientIdOrDefault(addModel.TimeZoneClientId)).Id;
            }

            UserPendingDTO userPending = await _userPendingService.CreateUpdateUserPendingAsync(modelDTO);

            if (company != null)
            {
                bool isDoaminAutoApproved;
                MailAddress address = new MailAddress(model.Email);
                string domainName = address.Host;
                bool IsUnApprovedDomain = Enum.GetNames(typeof(UnApprovedDomain)).Any(uad => domainName.ToUpper().Contains(uad));
                if (!IsUnApprovedDomain && (model.RoleId == RoleType.Internal || model.RoleId == RoleType.Corporation))
                {
                    bool isValidCompanyRole = true;
                    if (model.RoleId == RoleType.Corporation)
                    {
                        isValidCompanyRole = (company.TypeId == (int)CompanyType.Corporation);
                    }
                    isDoaminAutoApproved = _companyService.IsCompanyDomainExist(company.Id, domainName);
                    if (isDoaminAutoApproved && isValidCompanyRole)
                    {
                        try
                        {
                            user = await _userPendingService.ApproveUserPendingAsync(userPending!);
                            if (user.Id != 0)
                            {
                                List<int> adminUsersIds = await _userService.GetAdminUsersIdsAsync();
                                await _notificationService.AddNotificationsAsync(adminUsersIds, NotificationType.UserAutoApproved,
                                    new UserAutoApprovedNotificationDetails
                                    {
                                        UserId = user.Id,
                                        UserName = $"{modelDTO.FirstName} {modelDTO.LastName}",
                                        CompanyName = company?.Name
                                    });
                            }

                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation($"User {modelDTO.FirstName} {modelDTO.LastName} is not auto approved");
                        }
                    }
                }
            }

            if (id == 0 && user == null)
            {
                // notifications
                try
                {
                    List<int> adminUsersIds = await _userService.GetAdminUsersIdsAsync();
                    await _notificationService.AddNotificationsAsync(adminUsersIds, NotificationType.UserRegistered,
                        new UserRegisteredNotificationDetails
                        {
                            UserId = userPending.Id,
                            UserName = $"{modelDTO.FirstName} {modelDTO.LastName}"
                        });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
                }
            }

            return userPending.Id;
        }

        public async Task<int> ApproveUserPending(int id)
        {
            UserPendingDTO? userPendingDTO = await _userPendingService.GetUserPendingAsync(id, "company");
            if (userPendingDTO == null)
            {
                throw new CustomException("Pending user is not valid.");
            }
            if (userPendingDTO != null && !userPendingDTO.CompanyId.HasValue)
            {
                throw new CustomException("Company of pending user is not valid.");
            }
            if (userPendingDTO.RoleId != (int)RoleType.Internal)
            {
                if ((userPendingDTO.RoleId == (int)RoleType.Corporation &&
                    !(userPendingDTO.Company.TypeId == (int)CompanyType.Corporation)) ||
                    ((userPendingDTO.RoleId == (int)RoleType.SolutionProvider || userPendingDTO.RoleId == (int)RoleType.SPAdmin) &&
                    !(userPendingDTO.Company.TypeId == (int)CompanyType.SolutionProvider)))
                {
                    throw new CustomException("Role of pending user is not matching company role.");
                }
            }

            UserDTO userDTO = await _userPendingService.ApproveUserPendingAsync(userPendingDTO!);
            MailAddress address = new MailAddress(userPendingDTO.Email);
            string domainName = address.Host;
            bool IsUnApprovedDomain = Enum.GetNames(typeof(UnApprovedDomain)).Any(uad => domainName.ToUpper().Contains(uad));
            if (!IsUnApprovedDomain && userPendingDTO.RoleId == (int)RoleType.Internal || userPendingDTO.RoleId == (int)RoleType.Corporation)
            {
                int companyId = userPendingDTO.CompanyId != null ? (int)userPendingDTO.CompanyId : 0;
                bool isDOmainExist = _companyService.IsCompanyDomainExist(companyId, domainName);
                if (!isDOmainExist)
                {
                    await _companyService.CreateCompanyDomainAsync(companyId, domainName);
                }
            }
            return userDTO.Id;
        }

        public async Task<bool> DenyUserPending(int id, bool isDenied)
        {
            return await _userPendingService.DenyUserPendingAsync(id, isDenied);
        }

        public async Task<int> GetPendingUserCount()
        {
            return await _userPendingService.GetPendingUserCountAsync();
        }

        public async Task DeleteDeniedUserPendingsAsync()
        {
            await _userPendingService.DeleteDeniedUserPendingsAsync();
        }
    }
}