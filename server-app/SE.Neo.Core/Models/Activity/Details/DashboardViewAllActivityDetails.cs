using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class DashboardViewAllActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public DashboardViewAllActivityDetails(string viewAllResourceName) : base()
        {
            if (!Enum.TryParse(typeof(DashboardResourceViewAllType), viewAllResourceName, out _))
            {
                throw new ArgumentException();
            }

            ViewAllResourceName = viewAllResourceName;
        }

        public string ViewAllResourceName { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Dashboard
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.ViewAllClick
            };
        }
    }
}
