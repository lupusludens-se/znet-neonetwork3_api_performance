using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class LinkInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public LinkInfoActivityDetails(string name, string url) : base()
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(url))
            {
                throw new ArgumentException();
            }

            Name = name;
            Url = url;
        }

        public string Name { get; private set; }
        public string Url { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.ViewUserProfile,
                ActivityLocation.ViewCompanyProfile,
                ActivityLocation.ForumDetails
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.LinkClick
            };
        }
    }
}
