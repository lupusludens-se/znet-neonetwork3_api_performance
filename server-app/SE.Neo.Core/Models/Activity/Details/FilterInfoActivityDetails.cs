using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class FilterInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public FilterInfoActivityDetails(string filterDetails) : base()
        {
            if (filterDetails == null)
            {
                throw new ArgumentNullException(nameof(filterDetails));
            }

            FilterDetails = filterDetails;
        }

        public string FilterDetails { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.UserManagement,
                ActivityLocation.Learn,
                ActivityLocation.Forums,
                ActivityLocation.ProjectCatalog,
                ActivityLocation.Community
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.FilterApply
            };
        }
    }
}
