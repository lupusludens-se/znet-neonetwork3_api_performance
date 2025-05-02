using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class TechnologiesSolutionsActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public TechnologiesSolutionsActivityDetails(string? buttonName) : base()
        {
            buttonName = buttonName?.ToLower();
            if (buttonName != "technologies" && buttonName != "solutions")
            {
                throw new ArgumentException();
            }

            ButtonName = buttonName;
        }

        public string ButtonName { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Learn,
                ActivityLocation.ProjectCatalog
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.TechnologiesSolutionsClick
            };
        }
    }
}
