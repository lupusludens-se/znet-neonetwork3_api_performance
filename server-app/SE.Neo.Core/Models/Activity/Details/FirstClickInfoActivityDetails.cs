using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class FirstClickInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public FirstClickInfoActivityDetails(string actionType, int? id = null, bool isPublicUser = false, string title = "") : base()
        {
            if (!isPublicUser)
            {
                if (!Enum.TryParse(typeof(DashboardClickElementActionType), actionType, out _) || (id.HasValue && id <= 0))
                {
                    throw new ArgumentException();
                }
            }
            ActionType = ((DashboardClickElementActionType)Enum.Parse(typeof(DashboardClickElementActionType), actionType)).GetDescription();
            Id = id;
        }

        public string ActionType { get; private set; }
        public int? Id { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Dashboard
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.FirstDashboardClick
            };
        }
    }
}
