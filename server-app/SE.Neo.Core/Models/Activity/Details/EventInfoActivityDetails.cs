using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class EventInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public EventInfoActivityDetails(int eventId) : base()
        {
            if (eventId <= 0)
            {
                throw new ArgumentException();
            }

            EventId = eventId;
        }

        public int EventId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.EventDetails
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.EventDetailsView
            };
        }
    }
}
