using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class SolutionTypeInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public SolutionTypeInfoActivityDetails(int technologyId) : base()
        {
            if (technologyId <= 0)
            {
                throw new ArgumentException();
            }

            TechnologyId = technologyId;
        }

        public int TechnologyId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.SolutionTypes
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.SolutionTypes
            };
        }
    }
}

