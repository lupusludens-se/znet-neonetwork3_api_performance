using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Activity
{
    public class ActivityRequest
    {
        [EnumExist(typeof(ActivityType), "must contain valid activity type")]
        public ActivityType TypeId { get; set; }

        [EnumExist(typeof(ActivityLocation), "must contain valid activity location")]
        public ActivityLocation LocationId { get; set; }

        [ActivityDetailValid("TypeId", "LocationId")]
        public string Details { get; set; }
    }
}
