using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Activity;
using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities.TrackingActivity;
using SE.Neo.Core.Models.Activity.Details;
using SE.Neo.Core.Services.Interfaces;
using System.Net;

namespace SE.Neo.Core.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<IActivityService> _logger;
        private readonly IMapper _mapper;

        public ActivityService(ApplicationContext context, ILogger<IActivityService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<int> CreateActivityAsync(ActivityDTO modelDTO)
        {
            Activity activity = _mapper.Map<Activity>(modelDTO);
            await _context.Activities.AddAsync(activity);
            await _context.SaveChangesAsync();
            return activity.Id;
        }

        public async Task<int> CreatePublicActivityAsync(PublicSiteActivityDTO modelDTO)
        {
            PublicSiteActivity activity = _mapper.Map<PublicSiteActivity>(modelDTO);
            await _context.PublicSiteActivities.AddAsync(activity);
            await _context.SaveChangesAsync();
            return activity.Id;
        }

        public bool IsValid(string serializedValue, Enums.ActivityType activityType, Enums.ActivityLocation activityLocation)
        {
            Type detailsType = activityType.GetActivityDetailsType();
            IActivityDetails details;
            try
            {
                serializedValue = WebUtility.UrlDecode(serializedValue);
                details = (IActivityDetails)JsonConvert.DeserializeObject(serializedValue, detailsType)!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Activity details type for activity type {activityType.GetDescription()} is not valid.");
                return false;
            }
            return details.IsValid((int)activityType, (int)activityLocation);
        }
    }
}