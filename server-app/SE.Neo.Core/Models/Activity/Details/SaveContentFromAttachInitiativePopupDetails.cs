using SE.Neo.Core.Enums;
using SE.Neo.Common.Models.Activity.Details;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class SaveContentFromAttachInitiativePopupDetails : BaseActivityDetails, IActivityDetails
    {
        public SaveContentFromAttachInitiativePopupDetails(int[] initiativeIds, string moduleName, int contentId) : base()
        {
            if ((initiativeIds != null && initiativeIds.Length <= 0) || string.IsNullOrEmpty(moduleName) || contentId < 0)
            {
                throw new ArgumentException();
            }

            InitiativeIds = initiativeIds;
            ModuleName = moduleName;
            ContentId = contentId;
        }
        public int[] InitiativeIds { get; private set; }

        public string ModuleName { get; private set; }
        public int ContentId { get; private set; }
        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
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
                ActivityType.SaveContentFromAttachContentPopUp
            };
        }
    }
}
