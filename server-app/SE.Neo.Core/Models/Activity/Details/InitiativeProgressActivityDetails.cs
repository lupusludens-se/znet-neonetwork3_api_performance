using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class InitiativeProgressActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public InitiativeProgressActivityDetails(int initiativeId, int subStepId, string stepDescription) : base()
        {
            if (initiativeId <= 0 || subStepId < 0 || string.IsNullOrEmpty(stepDescription))
            {
                throw new ArgumentException();
            }

            InitiativeId = initiativeId;
            StepDescription = stepDescription;
            SubstepId = subStepId;
        }
        public int? InitiativeId { get; private set; }

        public int? SubstepId { get; private set; }

        public string StepDescription { get; private set; }
        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.ViewInitiative
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.InitiativeSubstepClick
            };
        }
    }
}
