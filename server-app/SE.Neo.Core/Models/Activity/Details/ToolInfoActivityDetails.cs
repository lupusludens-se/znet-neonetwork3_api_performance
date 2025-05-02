using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class ToolInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public ToolInfoActivityDetails(int toolId , int? initiativeId) : base()
        {
            if (toolId <= 0)
            {
                throw new ArgumentException();
            }

            ToolId = toolId;
            InitiativeId = initiativeId;
        }

        public int ToolId { get; private set; }
        public int? InitiativeId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Dashboard,
                ActivityLocation.Tools,
                ActivityLocation.ViewInitiative,
                ActivityLocation.CreateInitiative,
                ActivityLocation.InitiativeManageModulePage
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.ToolClick
            };
        }
    }
}
