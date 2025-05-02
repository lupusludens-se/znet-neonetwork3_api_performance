using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class UserInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public UserInfoActivityDetails(int userId, int? initiativeId) : base()
        {
            if (userId <= 0)
            {
                throw new ArgumentException();
            }

            UserId = userId;
            InitiativeId = initiativeId;
        }

        public int UserId { get; private set; }
        public int? InitiativeId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.ViewUserProfile,
                 ActivityLocation.ViewInitiative,
                 ActivityLocation.CreateInitiative,
                 ActivityLocation.InitiativeManageModulePage
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.UserProfileView
            };
        }
    }
}
