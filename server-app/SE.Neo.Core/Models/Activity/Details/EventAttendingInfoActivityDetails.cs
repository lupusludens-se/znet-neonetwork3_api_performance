using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class EventAttendingInfoActivityDetails : EventInfoActivityDetails, IActivityDetails
    {
        public EventAttendingInfoActivityDetails(int eventId, bool? doesAttend) : base(eventId)
        {
            DoesAttend = doesAttend;
        }

        public bool? DoesAttend { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Events,
                ActivityLocation.EventDetails
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.EventAttendingButtonClick
            };
        }
    }
}
