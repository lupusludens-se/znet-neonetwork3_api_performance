using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Feedback;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Feedback;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Globalization;
using System.Text;

namespace SE.Neo.WebAPI.Services
{
    public class FeedbackApiService : IFeedbackApiService
    {

        private readonly ILogger<FeedbackApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IFeedbackService _feedbackService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly ICompanyService _companyService;

        public FeedbackApiService(ILogger<FeedbackApiService> logger, IMapper mapper, IFeedbackService feedbackService, IUserService userService, INotificationService notificationService, ICompanyService companyService)
        {
            _logger = logger;
            _mapper = mapper;
            _feedbackService = feedbackService;
            _notificationService = notificationService;
            _userService = userService;
            _notificationService = notificationService;
            _companyService = companyService;
        }

        public async Task<WrapperModel<FeedbackResponse>> GetFeedbacksAsync(BaseSearchFilterModel filter)
        {
            WrapperModel<FeedbackDTO> feedbacks = await _feedbackService.GetFeedbacksAsync(filter);
            var wrapper = new WrapperModel<FeedbackResponse>
            {
                Count = feedbacks.Count,
                DataList = feedbacks.DataList.Select(_mapper.Map<FeedbackResponse>)
            };
            return wrapper;

        }

        public async Task<int> CreateFeedbackAsync(FeedbackRequest model, UserModel user)
        {
            CreateFeedbackDTO modelDTO = _mapper.Map<CreateFeedbackDTO>(model);
            modelDTO.UserId = user.Id;
            int feedbackId = await _feedbackService.CreateFeedbackAsync(modelDTO);
            if(feedbackId != 0)
            {
                List<int> adminUsersIds = await _userService.GetAdminUsersIdsAsync();
                CompanyDTO? company = await _companyService.GetCompanyAsync(user.CompanyId);
                await _notificationService.AddNotificationsAsync(adminUsersIds, NotificationType.NewFeedback,
                    new FeedbackNotificationDetails
                    {
                        UserId = user.Id,
                        UserName = $"{user.FirstName} {user.LastName}",
                        CompanyName = company?.Name,
                        CompanyId = user.CompanyId,
                        FeedbackId = feedbackId
                    });
            }
            return feedbackId;
        }

        public async Task<FeedbackResponse> GetFeedbackAsync(int feedbackId)
        {
            FeedbackDTO? feedback = await _feedbackService.GetFeedbackAsync(feedbackId);
            return _mapper.Map<FeedbackResponse>(feedback);

        }


        public async Task<int> ExportFeedbacksAsync(BaseSearchFilterModel filter, UserModel? currentuser, MemoryStream stream)
        {
            filter.FilterBy = $"{(filter.FilterBy ?? string.Empty)}";
            WrapperModel<FeedbackDTO> feedbacksResult = await _feedbackService.GetFeedbacksAsync(filter);
            List<FeedbackExportResponse> feedbackResponses = feedbacksResult.DataList.Select(_mapper.Map<FeedbackExportResponse>).ToList();

            using (var writeFile = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                using (var csv = new CsvWriter(writeFile, new CsvConfiguration(CultureInfo.InvariantCulture) { Encoding = Encoding.UTF8, LeaveOpen = true }))
                {
                    var classMap = new DefaultClassMap<FeedbackExportResponse>();
                    writeFile.Write($"Exported from Zeigo Network on {DateTime.Now.ToString("MM/dd/yyyy")}\n");
                    writeFile.Write($"Exported By User: {currentuser.FirstName} {currentuser.LastName} \n");
                    csv.NextRecord();

                    classMap.Map(o => o.User).Name("User").Index(0);
                    classMap.Map(o => o.Company).Name("Company").Index(1);
                    classMap.Map(o => o.Role).Name("Role").Index(2);
                    classMap.Map(o => o.Rating).Name("Ratings").Index(3);
                    classMap.Map(o => o.Comments).Name("Comments").Index(4);
                    classMap.Map(o => o.CreatedOn).Name("Date").Index(5);
                    csv.Context.RegisterClassMap(classMap);
                    csv.WriteRecords(feedbackResponses);
                }
                stream.Position = 0;
            }
            return feedbackResponses.Count;
        }
    }
}
