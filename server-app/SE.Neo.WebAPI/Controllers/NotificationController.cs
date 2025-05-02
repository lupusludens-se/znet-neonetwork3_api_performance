using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Notifications.Details.Multiple;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.EmailTemplates.Models;
using SE.Neo.EmailTemplates.Models.BaseModel;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Constants;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Attributes;
using SE.Neo.WebAPI.Constants;
using System.Web;

namespace SE.Neo.WebAPI.Controllers
{
    // Only for testing purpose. Remove it after notifications work properly.
    [ApiController]
    [Route("api/test")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;
        private readonly IEmailService _emailService;
        private readonly AzureB2CConfig _b2cAppConfig;
        private readonly AzureStorageConfig _azureStorageConfig;
        private readonly EmailAssetsConfig _emailAssetsConfig;

        public NotificationController(ILogger<NotificationController> logger,
            INotificationService notificationService,
            IEmailService emailService,
            IOptions<AzureB2CConfig> b2cAppConfigOptions,
            IOptions<AzureStorageConfig> azureStorageConfigOptions,
            IOptions<EmailAssetsConfig> emailAssetsConfigOptions)
        {
            _logger = logger;
            _notificationService = notificationService;
            _emailService = emailService;
            _b2cAppConfig = b2cAppConfigOptions.Value;
            _azureStorageConfig = azureStorageConfigOptions.Value;
            _emailAssetsConfig = emailAssetsConfigOptions.Value;
        }

        [DevTestApi]
        [HttpPost("CommentsMyTopic(1)-N1-notifications")]
        public async Task<IActionResult> AddN1Notification([FromQuery] int userId, [FromBody] UserTopicNotificationDetails details)
        {
            var notificationId = await _notificationService.AddNotificationAsync(userId, NotificationType.CommentsMyTopic, details);
            return Ok(notificationId);
        }

        [DevTestApi]
        [HttpPost("LikesMyTopic(2)-N3-notifications")]
        public async Task<IActionResult> AddN3Notification([FromQuery] int userId, [FromBody] UserTopicNotificationDetails details)
        {
            var notificationId = await _notificationService.AddNotificationAsync(userId, NotificationType.LikesMyTopic, details);
            return Ok(notificationId);
        }

        [DevTestApi]
        [HttpPost("RepliesToMyComment(3)-N5-notifications")]
        public async Task<IActionResult> AddN5Notification([FromQuery] int userId, [FromBody] UserTopicNotificationDetails details)
        {
            var notificationId = await _notificationService.AddNotificationAsync(userId, NotificationType.RepliesToMyComment, details);
            return Ok(notificationId);
        }

        [DevTestApi]
        [HttpPost("RepliesToTopicIFollow(4)-N7-notifications")]
        public async Task<IActionResult> AddN7Notification([FromQuery] List<int> userIds, [FromBody] UserTopicNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.RepliesToTopicIFollow, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("MentionsMeInComment(5)-N9-notifications")]
        public async Task<IActionResult> AddN9Notification([FromQuery] List<int> userIds, [FromBody] UserTopicNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.MentionsMeInComment, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("InvitesMeToEvent(6)-N11-notifications")]
        public async Task<IActionResult> AddN11Notification([FromQuery] List<int> userIds, [FromBody] EventNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.InvitesMeToEvent, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("ChangesToEventIInvited(7)-N12-notifications")]
        public async Task<IActionResult> AddN12Notification([FromQuery] List<int> userIds, [FromBody] ChangeEventNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.ChangesToEventIInvited, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("ChangesToEventIInvited(7)-N13-notifications")]
        public async Task<IActionResult> AddN13MultipleNotification([FromQuery] List<int> userIds, [FromBody] EventNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.ChangesToEventIInvited, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("FollowsMe(8)-N14-notifications")]
        public async Task<IActionResult> AddN14Notification([FromQuery] int userId, [FromBody] FollowerNotificationDetails details)
        {
            var notificationId = await _notificationService.AddNotificationAsync(userId, NotificationType.FollowsMe, details);
            return Ok(notificationId);
        }

        [DevTestApi]
        [HttpPost("MessagesMe(9)-N16-notifications")]
        public async Task<IActionResult> AddN16Notification([FromQuery] int userId, [FromBody] MessageNotificationDetails details)
        {
            var notificationId = await _notificationService.AddNotificationAsync(userId, NotificationType.MessagesMe, details);
            return Ok(notificationId);
        }

        [DevTestApi]
        [HttpPost("AdminAlert(10)-N18-notifications")]
        public async Task<IActionResult> AddN18Notification([FromQuery] List<int> userIds, [FromBody] AdminAlertNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.AdminAlert, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("ChangesToProjectISaved(11)-N19-notifications")]
        public async Task<IActionResult> AddN19Notification([FromQuery] List<int> userIds, [FromBody] ChangeProjectNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.ChangesToProjectISaved, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("ChangesToProjectISaved(11)-N20-notifications")]
        public async Task<IActionResult> AddN20Notification([FromQuery] List<int> userIds, [FromBody] ProjectNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.ChangesToProjectISaved, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("CompanyIFollowPostProject(12)-N21-notifications")]
        public async Task<IActionResult> AddN21Notification([FromQuery] List<int> userIds, [FromBody] CompanyProjectNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.CompanyIFollowPostProject, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("UserRegistered(13)-N23-notifications")]
        public async Task<IActionResult> AddN23Notification([FromQuery] List<int> userIds, [FromBody] UserRegisteredNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.UserRegistered, details);
            return Ok(notificationIds);
        }

        [DevTestApi]
        [HttpPost("CompanyIFollowAddEmployee(14)-N25-notifications")]
        public async Task<IActionResult> AddN25Notification([FromQuery] List<int> userIds, [FromBody] CompanyEmployeeNotificationDetails details)
        {
            var notificationIds = await _notificationService.AddNotificationsAsync(userIds, NotificationType.CompanyIFollowAddEmployee, details);
            return Ok(notificationIds);
        }

        /// <summary>
        /// Send Test email
        /// </summary>
        /// <remarks>
        /// Email Type possible options - complete_profile, event_invitation, forum_response, new_message, summary
        /// <br />
        /// EmailTo - address of recipient
        /// </remarks>
        [DevTestApi]
        [HttpPost("EmailSending")]
        public async Task<IActionResult> EmailSendTesting([FromQuery] string emailType, [FromQuery] string emailTo)
        {
            string userName = "Zahra";
            string emailSubject = string.Empty;
            BaseTemplatedEmailModel model = null;
            const string linkTemplate = "{0}/{1}/oauth2/v2.0/authorize?client_id={2}&scope=openid%20profile%20offline_access&redirect_uri={3}&client-request-id=315f835b-61d2-4b19-a94d-530b0d67553b&response_mode=fragment&response_type=code&x-client-SKU=msal.js.browser&x-client-VER=2.22.0&x-client-OS=&x-client-CPU=&client_info=1&code_challenge=fbgtW3QkqECTPhA54ntrxLYAptu7_zt6WIwWJtCuWxs&code_challenge_method=S256&nonce=cb472ce7-c6ae-417f-baea-3c69e3d9f2ba&state=eyJpZCI6IjA2MjJhZjQyLTgxMzUtNGEzYy05NmVhLWY4ZWZkMDIwYzQ4NCIsIm1ldGEiOnsiaW50ZXJhY3Rpb25UeXBlIjoicmVkaXJlY3QifX0%3D";

            try
            {
                switch (emailType.ToLower())
                {
                    case "complete_profile":
                        {
                            emailSubject = EmailSubjects.CompleteProfile;

                            model = new CompleteProfileEmailTemplatedModel
                            {
                                LogoUrl = $"{_emailAssetsConfig.NeoLogo}",
                                BoldFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.BoldFont}",
                                LightFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.LightFont}",
                                RegularFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.RegularFont}",
                                FirstName = userName,
                                Link = string.Format(linkTemplate,
                                $"{_b2cAppConfig.Instance}/{_b2cAppConfig.Domain}",
                                _b2cAppConfig.ReserPasswordPolicyId,
                                _b2cAppConfig.AppClientId,
                                HttpUtility.UrlEncode(_b2cAppConfig.RedirectUrl))
                            };

                            break;
                        }
                    case "event_invitation":
                        {
                            emailSubject = EmailSubjects.EventInvitation;

                            model = new EventInvitationEmailTemplatedModel
                            {
                                LogoUrl = $"{_emailAssetsConfig.NeoLogo}",
                                BoldFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.BoldFont}",
                                LightFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.LightFont}",
                                RegularFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.RegularFont}",
                                FirstName = userName,
                                Link = string.Format(linkTemplate,
                                $"{_b2cAppConfig.Instance}/{_b2cAppConfig.Domain}",
                                _b2cAppConfig.ReserPasswordPolicyId,
                                _b2cAppConfig.AppClientId,
                                HttpUtility.UrlEncode(_b2cAppConfig.RedirectUrl)),
                                EventType = "Virtual Event",
                                EventInfo = "Curabitur non hendrerit ex. Proin posuere pharetra mauris at posuere. Nunc fringilla ex mollis, convallis augue ac, maximus dolor. Maecenas maximus convallis purus in tincidunt. Pellentesque aliquam augue urna, tempor imperdiet eros hendrerit a. Vivamus vitae hendrerit dui, at efficitur nibh. Nunc volutpat porttitor sagittis. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Pellentesque pulvinar et nulla non ullamcorper. Duis nibh tortor, tristique non sagittis a, viverra a mauris. Integer quis dolor mi.",
                                EventName = "Successful solar output in Central Europe.",
                                EventDateLogoUrl = $"{_emailAssetsConfig.EventsDate}",
                                EventTimeLogoUrl = $"{_emailAssetsConfig.EventInvitationTimer}",
                                EventHighlights = new List<string>
                            {
                                "Hot topics, opportunities and challenges specific to NJ, IL, NY, CA, IN, CT, PA, ND and CO",
                                "Internal stakeholder engagement",
                                "Financing consideration",
                                "And more"
                            },
                                EventDates = new List<EventDateInfo>
                            {
                                new EventDateInfo
                                {
                                    EventDate = DateTime.Today,
                                    EventStart = "10:00am",
                                    EventEnd = "10:30am",
                                    TimeZoneAbbreviation = "EST"
                                },
                                new EventDateInfo
                                {
                                    EventDate = DateTime.Today.AddDays(1),
                                    EventStart = "9:30am",
                                    EventEnd = "5:45pm",
                                    TimeZoneAbbreviation = "EDT"
                                }
                            }
                            };

                            break;
                        }
                    case "forum_response":
                        {
                            emailSubject = EmailSubjects.TopicResponse;

                            model = new ForumResponseEmailTemplatedModel
                            {
                                LogoUrl = $"{_emailAssetsConfig.NeoLogo}",
                                BoldFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.BoldFont}",
                                LightFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.LightFont}",
                                RegularFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.RegularFont}",
                                FirstName = userName,
                                Link = string.Format(linkTemplate,
                                $"{_b2cAppConfig.Instance}/{_b2cAppConfig.Domain}",
                                _b2cAppConfig.ReserPasswordPolicyId,
                                _b2cAppConfig.AppClientId,
                                HttpUtility.UrlEncode(_b2cAppConfig.RedirectUrl)),
                                ResponseAuthor = "Terry Kenter",
                                ForumTopic = "Successful solar output in Central Europe.",
                                ResponseText = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown."
                            };

                            break;
                        }
                    case "new_message":
                        {
                            emailSubject = EmailSubjects.NewMessage;

                            model = new NewMessageEmailTemplatedModel
                            {
                                LogoUrl = $"{_emailAssetsConfig.NeoLogo}",
                                BoldFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.BoldFont}",
                                LightFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.LightFont}",
                                RegularFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.RegularFont}",
                                FirstName = userName,
                                Link = string.Format(linkTemplate,
                                $"{_b2cAppConfig.Instance}/{_b2cAppConfig.Domain}",
                                _b2cAppConfig.ReserPasswordPolicyId,
                                _b2cAppConfig.AppClientId,
                                HttpUtility.UrlEncode(_b2cAppConfig.RedirectUrl)),
                                messages = { new MessageEmailTemplateModel {
                                MessageAuthor = "Terry Kenter",
                                MessageText = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown."
                                }}
                            };

                            break;
                        }
                    case "summary":
                        {
                            emailSubject = EmailSubjects.ForumSummary;

                            model = new SummaryEmailTemplatedModel
                            {
                                LogoUrl = $"{_emailAssetsConfig.NeoLogo}",
                                BoldFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.BoldFont}",
                                LightFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.LightFont}",
                                RegularFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.RegularFont}",
                                FirstName = userName,
                                Link = string.Format(linkTemplate,
                                $"{_b2cAppConfig.Instance}/{_b2cAppConfig.Domain}",
                                _b2cAppConfig.ReserPasswordPolicyId,
                                _b2cAppConfig.AppClientId,
                                HttpUtility.UrlEncode(_b2cAppConfig.RedirectUrl)),
                                Items = new List<SummaryEmailItem>
                            {
                                new SummaryEmailItem
                                {
                                    ItemTypeName = "Projects",
                                    MainTitle = "Large scale (3 MW+) groundmount solar opportunity in IL (installed at your site)",
                                    ItemTypeLogoUrl = $"{_emailAssetsConfig.ProjectsLogo}",
                                    MainText = "Installing solar panels at your facility announces a visible commitment to carbon reduction. Project economics and viability will.",
                                    Tags = new List<string>
                                    {
                                        "Solar (Onsite)"
                                    },
                                    Regions = new List<string>
                                    {
                                        "US & Canada", "US - California"
                                    }
                                },
                                new SummaryEmailItem
                                {
                                    ItemTypeName = "Forum",
                                    MainTitle = "How do leases for onsite solar systems work in Florida? What are the risks vs. doing a PPA in states where they’re allowed?",
                                    ItemTypeLogoUrl = $"{_emailAssetsConfig.ForumLogo}",
                                    MainText = "<p>By clicking \"Accept\" or continuing to use our site, you agree to our Privacy Policy for Website<span class=\"cb-enable bg-white text-dark py-1 px-3 ml-2 rounded cp\">Accept</span><a href=\"https://smallseotools.com/privacy/\" class=\"cb-policy\">Privacy Policy</a></p>",
                                    Tags = new List<string>
                                    {
                                        "Offsite Power Purchase Agreement", "Efficiency Equipment Measures", "Renewable Retail Electricity"
                                    },
                                    Regions = new List<string>
                                    {
                                        "US & Canada", "US - California", "Japan", "Africa", "Europa"
                                    }
                                },
                                new SummaryEmailItem
                                {
                                    ItemTypeName = "Forum",
                                    MainTitle = "How do leases for onsite solar systems work in Florida? How do leases for onsite solar systems work in Florida? What are the risks vs. doing a PPA in states where they’re allowed?",
                                    ItemTypeLogoUrl = $"{_emailAssetsConfig.ForumLogo}",
                                    MainText = "Nulla sed eros neque. Mauris ultrices ante a dolor consectetur ullamcorper. Donec pellentesque nisi eu quam dapibus ullamcorper. Proin vel condimentum felis. Fusce eget elit.",
                                    Tags = new List<string>
                                    {
                                        "Solar (Onsite)", "Efficiency Equipment Measures"
                                    },
                                    Regions = new List<string>
                                    {
                                        "US & Canada", "US - California", "Japan", "Africa", "Europa"
                                    }
                                },
                                new SummaryEmailItem
                                {
                                    ItemTypeName = "Events",
                                    MainTitle = "Successful solar output in Central Europe",
                                    ItemTypeLogoUrl = $"{_emailAssetsConfig.EventsLogo}",
                                    MainText = "Virtual Workshop",
                                    EventDateInfo = new EventDateInfo
                                    {
                                        EventDate = DateTime.Today,
                                        EventStart = "3pm",
                                        EventEnd = "5pm"
                                    },
                                    EventDateLogoUrl = $"{_emailAssetsConfig.EventsDate}",
                                    EventTimeLogoUrl = $"{_emailAssetsConfig.EventsTime}"
                                },
                                new SummaryEmailItem
                                {
                                    ItemTypeName = "Learn",
                                    MainTitle = "4 Key Challenges & Solutions to Achieving Carbon Neutrality",
                                    ItemTypeLogoUrl = $"{_emailAssetsConfig.LearnLogo}",
                                    Tags = new List<string>
                                    {
                                        "Solar (Onsite)",
                                        "Fuel Cells"
                                    },
                                    Regions = new List<string>
                                    {
                                        "US & Canada", "US - California"
                                    }
                                }
                            }
                            };

                            break;
                        }
                    default:
                        return NotFound();
                }

                await _emailService.SendTemplatedEmailAsync(emailTo, emailSubject, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ErrorMessages.ErrorAddingNotification);
            }

            return Ok();
        }
    }
}
