using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class ProjectResourceActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public ProjectResourceActivityDetails(int resourceId) : base()
        {
            if (resourceId <= 0)
            {
                throw new ArgumentException();
            }

            ResourceId = resourceId;
        }

        public int ResourceId { get; set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.ProjectDetails
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.ProjectResourceClick
            };
        }
    }
}
