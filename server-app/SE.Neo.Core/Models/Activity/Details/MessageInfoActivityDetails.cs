using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class MessageInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public MessageInfoActivityDetails(int messageId, int? initiativeId) : base()
        {
            if (messageId <= 0)
            {
                throw new ArgumentException();
            }

            MessageId = messageId;
            InitiativeId = initiativeId;
        }

        public int MessageId { get; private set; }
        public int? InitiativeId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Forums,
                ActivityLocation.ForumDetails,
                ActivityLocation.CreateInitiative,
                ActivityLocation.ViewInitiative,
                ActivityLocation.InitiativeManageModulePage
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.ForumMessageLike,
                ActivityType.ForumMessageResponse,
                ActivityType.MessageDetailsView
            };
        }
    }
}
