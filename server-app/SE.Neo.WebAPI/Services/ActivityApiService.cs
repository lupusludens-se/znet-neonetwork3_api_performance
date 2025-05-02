using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SE.Neo.Common;
using SE.Neo.Common.Models.Activity;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Activity;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class ActivityApiService : IActivityApiService
    {
        private readonly ILogger<ActivityApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IActivityService _activityService;
        private readonly IHttpContextAccessor _httpContext;

        public ActivityApiService(ILogger<ActivityApiService> logger, IMapper mapper, IActivityService activityService, IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _mapper = mapper;
            _activityService = activityService;
            _httpContext = httpContext;
        }

        public async Task<int> CreateActivityAsync(ActivityRequest model)
        {
            _logger.LogInformation($"Activity Model is {JsonConvert.SerializeObject(model)}");
            var activityDTO = _mapper.Map<ActivityDTO>(model);
            activityDTO.SessionId = (Guid)_httpContext.HttpContext!.Items["SessionId"]!;
            return await _activityService.CreateActivityAsync(activityDTO);
        }

        public async Task<int> CreatePublicActivityAsync(ActivityRequest model)
        {
            var jsonObject = JObject.Parse(model.Details);
            if (jsonObject.ContainsKey(ZnConstants.isPublicUser))
            {
                jsonObject.Remove(ZnConstants.isPublicUser);
            }
            model.Details = Convert.ToString(jsonObject);

            var publicActivityDTO = _mapper.Map<PublicSiteActivityDTO>(model);
            return await _activityService.CreatePublicActivityAsync(publicActivityDTO);
        }
    }
}
