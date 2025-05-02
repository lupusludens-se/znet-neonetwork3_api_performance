using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class CompanyInfoActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public CompanyInfoActivityDetails(int companyId, bool isPublicUser = false, string companyName = "") : base()
        {
            if (!isPublicUser)
            {
                if (companyId <= 0)
                {
                    throw new ArgumentException();
                }
                CompanyId = companyId;
            }
            else
            {
                if (string.IsNullOrEmpty(companyName))
                {
                    throw new ArgumentException();
                }
            }
        }

        public int CompanyId { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.Community,
                ActivityLocation.ProjectDetails,
                ActivityLocation.ViewCompanyProfile,
                ActivityLocation.EditCompany
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.CompanyProfileView,
                ActivityType.CompanyFollow,
                ActivityType.ContactProviderClick,
                ActivityType.ContactProviderNevermindButtonClick,
                ActivityType.ContactProviderConfirmButtonClick
            };
        }
    }
}
