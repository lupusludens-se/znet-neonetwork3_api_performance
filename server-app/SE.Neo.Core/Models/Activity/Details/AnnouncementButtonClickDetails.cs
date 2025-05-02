using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class AnnouncementButtonClickDetails : BaseActivityDetails, IActivityDetails
    {

        public AnnouncementButtonClickDetails(string actionType, int id, bool isPublicUser = false, string title = "") : base()
        {
            if (!isPublicUser)
            {
                if (!Enum.TryParse(typeof(DashboardClickElementActionType), actionType, out _) || (id <= 0))
                {
                    throw new ArgumentException();
                }
            }
            Id = id;
        }
        public int Id { get; set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Dashboard
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.AnnouncementButtonClick
            };
        }
    }
}
