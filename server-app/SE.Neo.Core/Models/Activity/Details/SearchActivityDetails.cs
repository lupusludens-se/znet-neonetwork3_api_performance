using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class SearchActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public SearchActivityDetails(string searchTerm) : base()
        {
            if (searchTerm == null)
            {
                throw new ArgumentException();
            }

            SearchTerm = searchTerm;
        }

        public string SearchTerm { get; private set; }

        public override bool IsValid(int type, int location)
        {
            return typeWhiteList.Contains((ActivityType)type);
        }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.AdmitUsers,
                ActivityLocation.Community,
                ActivityLocation.CompanyManagement,
                ActivityLocation.Events,
                ActivityLocation.Forums,
                ActivityLocation.Learn,
                ActivityLocation.Messages,
                ActivityLocation.ProjectCatalog,
                ActivityLocation.ProjectLibrary,
                ActivityLocation.SavedContent,
                ActivityLocation.ToolManagement,
                ActivityLocation.Tools,
                ActivityLocation.UserManagement
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.SearchApply
            };
        }
    }
}
