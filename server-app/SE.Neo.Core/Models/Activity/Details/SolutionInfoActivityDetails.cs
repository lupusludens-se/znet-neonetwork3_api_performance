using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class SolutionInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public SolutionInfoActivityDetails(int solutionId) : base()
        {
            if (solutionId <= 0)
            {
                throw new ArgumentException();
            }

            SolutionId = solutionId;
        }

        public int SolutionId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.SolutionDetails
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.SolutionDetailsView
            };
        }
    }
}
