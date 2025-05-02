using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class ActicleInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public ActicleInfoActivityDetails(int articleId, string articleName, int? initiativeId) : base()
        {
            if (articleId <= 0 || string.IsNullOrEmpty(articleName))
            {
                throw new ArgumentException();
            }

            ArticleId = articleId;
            ArticleName = articleName;
            InitiativeId = initiativeId;
        }

        public int ArticleId { get; private set; }

        public string ArticleName { get; private set; }

        public int? InitiativeId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.LearnDetails,
                ActivityLocation.Learn,
                ActivityLocation.Dashboard,
                ActivityLocation.ViewInitiative,
                ActivityLocation.CreateInitiative,
                ActivityLocation.InitiativeManageModulePage
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.LearnView,
                ActivityType.PrivateLearnClick,ActivityType.FirstDashboardClick
            };
        }
    }
}
