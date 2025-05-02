using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class EventRegistrationActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public EventRegistrationActivityDetails(int eventId) : base()
        {
            if (eventId <= 0)
            {
                throw new ArgumentException();
            }

            EventId = eventId;
        }

        public int EventId { get; set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.EventDetails
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.EventRegistration
            };
        }
    }
}
