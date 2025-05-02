using SE.Neo.Common.Models.Activity.Details;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Models.Activity.Details
{
    public abstract class BaseActivityDetails : IActivityDetails
    {
        protected ActivityType[] typeWhiteList;
        protected ActivityLocation[] locationWhiteList;

        public BaseActivityDetails()
        {
            this.InitAvailableTypes();
            this.InitAvailableLocations();
        }

        public virtual bool IsValid(int type, int location)
        {
            return typeWhiteList.Contains((ActivityType)type) && locationWhiteList.Contains((ActivityLocation)location);
        }

        public abstract void InitAvailableTypes();

        public abstract void InitAvailableLocations();
    }
}
