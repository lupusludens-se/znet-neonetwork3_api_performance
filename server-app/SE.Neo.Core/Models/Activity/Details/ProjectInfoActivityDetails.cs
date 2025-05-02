using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class ProjectInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public ProjectInfoActivityDetails(int projectId, int? initiativeId, int? companyId)
        {
            if (projectId <= 0)
            {
                throw new ArgumentException();
            }

            ProjectId = projectId;
            InitiativeId = initiativeId;
            CompanyId = companyId;
        }

        public int ProjectId { get; private set; }
        public int? InitiativeId { get; private set; }

        public int? CompanyId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Dashboard,
                ActivityLocation.ProjectDetails,
                ActivityLocation.ProjectCatalog,
                ActivityLocation.EditProject,
                ActivityLocation.ViewInitiative,
                ActivityLocation.CreateInitiative,
                ActivityLocation.InitiativeManageModulePage,
                ActivityLocation.ViewCompanyProfile
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.ProjectSave,
                ActivityType.ProjectView
            };
        }
    }
}
