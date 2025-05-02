using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class InitiativeInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {

        public InitiativeInfoActivityDetails(int initiativeId, string initiativeTitle, string? resourceType) : base()
        {
            if (initiativeId <= 0 || string.IsNullOrEmpty(initiativeTitle))
            {
                throw new ArgumentException();
            }

            InitiativeId = initiativeId;
            InitiativeTitle = initiativeTitle;
            ResourceType = resourceType;
        }
        public int? InitiativeId { get; private set; }

        public string InitiativeTitle { get; private set; }
        public string ResourceType { get; private set; }
        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.ViewInitiative,
                ActivityLocation.Dashboard
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.InitiativeCreateClick,
                ActivityType.InitiativeDetailsView,
                ActivityType.InitiativeSubstepClick,
                ActivityType.InitiativeModuleViewAllClick
            };
        }
    }
}
