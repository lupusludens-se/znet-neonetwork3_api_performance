using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using SE.Neo.WebAPI.Models.UserProfile;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class UserProfileApiService : IUserProfileApiService
    {
        private readonly ILogger<UserProfileApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserProfileService _userProfileService;
        private readonly ICompanyService _companyService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IDotDigitalService _dotDigitalService;

        public UserProfileApiService(ILogger<UserProfileApiService> logger, IMapper mapper, IUserProfileService userProfileService, ICompanyService companyService, INotificationService notificationService, IUserService userService, IDotDigitalService dotDigitalService)
        {
            _logger = logger;
            _mapper = mapper;
            _userProfileService = userProfileService;
            _companyService = companyService;
            _notificationService = notificationService;
            _userService = userService;
            _dotDigitalService = dotDigitalService;
        }

        public async Task<WrapperModel<UserProfileResponse>> GetUserProfilesAsync(BaseSearchFilterModel filter, int userId)
        {
            WrapperModel<UserProfileDTO> userProfilesResult = await _userProfileService.GetUserProfilesAsync(filter, userId);

            var wrapper = new WrapperModel<UserProfileResponse>
            {
                Count = userProfilesResult.Count,
                DataList = userProfilesResult.DataList.Select(_mapper.Map<UserProfileResponse>)
            };
            return wrapper;
        }

        public async Task<UserProfileResponse?> GetUserProfileAsync(int id, int userId, string? expand)
        {
            var modelDTO = await _userProfileService.GetUserProfileAsync(id, userId, expand);
            return _mapper.Map<UserProfileResponse>(modelDTO);
        }

        public async Task<int> CreateUpdateUserProfileAsync(int id, UserProfileRequest model, string? ConsentIp = "", string? ConsentUserAgent = "",bool? isEditCurrentProfile=false)
        {
            var modelDTO = _mapper.Map<UserProfileDTO>(model);
            var userId = await _userProfileService.CreateUpdateUserProfileAsync(id, modelDTO, isEditCurrentProfile);

            UserDTO userDTO = await _userService.GetUserAsync(userId, "company,country");

            _logger.LogInformation($"Dotdigital Mail Id: {userDTO.Email} - {model.AcceptWelcomeSeriesEmail}");

            if (modelDTO.AcceptWelcomeSeriesEmail)
            {
                _dotDigitalService.CreateContactAndAddUserToAddressBook(userDTO, ConsentIp, ConsentUserAgent);
            }

            if (id == 0)
            {
                // notifications
                try
                {
                    List<int> companyFollowersIds = await _companyService.GetCompanyFollowersIdsAsync(userDTO!.CompanyId);
                    await _notificationService.AddNotificationsAsync(companyFollowersIds, NotificationType.CompanyIFollowAddEmployee,
                        new CompanyEmployeeNotificationDetails
                        {
                            UserId = userDTO.Id,
                            UserName = $"{userDTO.FirstName} {userDTO.LastName}",
                            CompanyId = userDTO.CompanyId,
                            CompanyName = userDTO.Company.Name
                        });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
                }
            }

            return userId;
        }

        public async Task CreateUserProfileInterestAsync(int userId, UserProfileInterestRequest model)
        {
            var modelDTO = _mapper.Map<TaxonomyDTO>(model);
            await _userProfileService.CreateUserProfileInterestAsync(userId, modelDTO);
        }

        public async Task<UserProfileSuggestionResponse> GetUserProfileSuggestionsAsync(int userId)
        {
            var modelDTO = await _userProfileService.GetUserProfileSuggestionsAsync(userId);
            return _mapper.Map<UserProfileSuggestionResponse>(modelDTO);
        }



        public async Task<int> SyncUserLoginCountAsync()
        {
            return await _userProfileService.SyncUserLoginCountAsync();
        }

    }
}