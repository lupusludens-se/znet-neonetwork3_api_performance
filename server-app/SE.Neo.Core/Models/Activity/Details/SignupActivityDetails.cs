using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class SignupActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public SignupActivityDetails(string source) : base()
        {
            Source = source;
        }

        public string Source { get; private set; }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] {
                ActivityLocation.RegistrationPage
            };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.SignupClick
            };
        }
    }
}
