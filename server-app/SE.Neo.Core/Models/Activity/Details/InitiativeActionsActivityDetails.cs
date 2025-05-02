using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class InitiativeActionsActivityDetails : BaseActivityDetails, IActivityDetails
    {

        public InitiativeActionsActivityDetails(int? initiativeId, string buttonName, string? moduleName) : base()
        {
            if (initiativeId <= 0 || string.IsNullOrEmpty(buttonName))
            {
                throw new ArgumentException();
            }

            InitiativeId = initiativeId;
            ButtonName = buttonName;
            ModuleName = moduleName;
        }
        public int? InitiativeId { get; private set; }

        public string ButtonName { get; private set; }
        public string? ModuleName { get; private set; }
        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.CreateInitiative,
                ActivityLocation.ViewInitiative,
                ActivityLocation.InitiativeManageModulePage,
                ActivityLocation.LearnDetails,
                ActivityLocation.ProjectDetails,
                ActivityLocation.ViewTool,
                ActivityLocation.ViewUserProfile,
                ActivityLocation.ViewMessage
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.InitiativesButtonClick
            };
        }
    }
}

