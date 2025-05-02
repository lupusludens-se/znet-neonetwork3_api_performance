using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class EmptyActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public EmptyActivityDetails() : base()
        {
        }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Forums,
                ActivityLocation.ForumDetails,
                ActivityLocation.Learn,
                ActivityLocation.ProjectCatalog,
                ActivityLocation.ProjectDetails,
                ActivityLocation.ProjectLibrary,
                ActivityLocation.Dashboard,
                ActivityLocation.ViewTool,
                ActivityLocation.ViewUserProfile,
                ActivityLocation.ViewMessage,
                ActivityLocation.LearnDetails
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.NewDiscussionClick,
                ActivityType.ViewMapClick,
                ActivityType.ConnectWithNEOClick,
                ActivityType.NewProjectClick,
                ActivityType.LinkButtonClick,
                ActivityType.InitiativeCreateClick
            };
        }
    }
}
