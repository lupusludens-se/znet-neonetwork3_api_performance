using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public class NavMenuItemActivityDetails : BaseActivityDetails, IActivityDetails
    {
        public NavMenuItemActivityDetails(string navMenuItemName)
        {
            if (!Enum.TryParse(typeof(NavMenuItem), navMenuItemName, out _))
            {
                throw new ArgumentException();
            }

            NavMenuItemName = navMenuItemName;
        }

        public string NavMenuItemName { get; private set; }

        public override bool IsValid(int type, int location)
        {
            return typeWhiteList.Contains((ActivityType)type);
        }

        public override void InitAvailableLocations()
        {
            locationWhiteList = new ActivityLocation[] { };
        }

        public override void InitAvailableTypes()
        {
            typeWhiteList = new ActivityType[] {
                ActivityType.NavMenuItemClick
            };
        }
    }
}
