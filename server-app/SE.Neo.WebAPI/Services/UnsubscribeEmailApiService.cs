using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Models.Unsubscribe;
using SE.Neo.WebAPI.Models.UserProfile;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Web;

namespace SE.Neo.WebAPI.Services
{
    public class UnsubscribeEmailApiService : IUnsubscribeEmailApiService
    {
        private readonly ILogger<UnsubscribeEmailApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnsubscribeEmailService _unsubscribeEmailService;
        private readonly IUserService _userService;

        public UnsubscribeEmailApiService(ILogger<UnsubscribeEmailApiService> logger, IMapper mapper, IUnsubscribeEmailService unsubscribeEmailService,
            IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _unsubscribeEmailService = unsubscribeEmailService;
            _userService = userService;
        }

        public async Task<UnsubscribeResponse> GetEmailFromRequestToken(UnsubscribeRequest model)
        {
            UnsubscribeResponse response = new() { Message = "Invalid" };
            try
            {
                var decodedToken = model.Token;
                //doing double decoding because sometime the param is double encoded if the url again manually opened
                while (decodedToken.Contains("%"))
                {
                    decodedToken = HttpUtility.UrlDecode(decodedToken);
                }
                var userId = await Task.Run(() => _unsubscribeEmailService.DecryptAsync(Convert.FromBase64String(decodedToken)));
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _userService.GetUserByUserIdAsync(Convert.ToInt32(userId));
                    if (!string.IsNullOrEmpty(user?.Email))
                    {
                        response.EmailPreference = user.UserEmailAlerts.Where(uea => uea.EmailAlert.Id == (int)EmailAlertCategory.Summary)?.FirstOrDefault()?.Frequency;
                        response.Email = user.Email;
                        response.Message = "Success";
                    }
                    else
                    {
                        response.Message = "AlreadyUnsubscribed";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error while getemail from token. Error: {ex.Message}");
                _logger.LogInformation($"Error while getemail from token. Inner Error: {ex?.InnerException?.Message}");
            }
            return response;
        }

        public async Task<UnsubscribeResponse> UpdateEmailFrequency(UnsubscribeRequest model, EmailAlertCategory emailAlertCategory)
        {
            UnsubscribeResponse response = new() { Message = "Invalid" };
            try
            {
                var decodedToken = model.Token;
                //doing double decoding because sometime the param is double encoded if the url again manually opened
                while (decodedToken.Contains("%"))
                {
                    decodedToken = HttpUtility.UrlDecode(decodedToken);
                }
                var userId = await Task.Run(() => _unsubscribeEmailService.DecryptAsync(Convert.FromBase64String(decodedToken)));
                if (!string.IsNullOrEmpty(userId))
                {
                    var user = await _userService.GetUserByUserIdAsync(Convert.ToInt32(userId));
                    if (!string.IsNullOrEmpty(user?.Email))
                    {
                        var updateResponse = await _userService.UpdateUserEmailPreference(user.Id, emailAlertCategory, model.Frequency);
                        if (updateResponse)
                        {
                            response.Message = "Success";
                        }
                    }
                    else
                    {
                        response.Message = "AlreadyUnsubscribed";
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Error while update email frequency from token. Error: {ex.Message}");
                _logger.LogInformation($"Error while update email frequency from token. Inner Error: {ex?.InnerException?.Message}");
            }


            return response;
        }
    }
}