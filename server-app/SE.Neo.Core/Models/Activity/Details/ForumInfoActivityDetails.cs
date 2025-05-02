using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class ForumInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public ForumInfoActivityDetails(int forumId) : base()
        {
            if (forumId <= 0)
            {
                throw new ArgumentException();
            }

            ForumId = forumId;
        }

        public int ForumId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.ForumDetails,
                ActivityLocation.Forums
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.ForumFollow,
                ActivityType.ForumSave,
                ActivityType.ForumView
            };
        }
    }
}
