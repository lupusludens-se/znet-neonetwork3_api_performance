using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class FollowerInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public FollowerInfoActivityDetails(int followerId) : base()
        {
            if (followerId <= 0)
            {
                throw new ArgumentException();
            }

            FollowerId = followerId;
        }

        public int FollowerId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Community,
                ActivityLocation.ViewCompanyProfile,
                ActivityLocation.ViewUserProfile,
                ActivityLocation.EventDetails
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.UserFollow
            };
        }
    }
}
