namespace SE.Neo.Common.Models.Activity.Details
{
    public interface IActivityDetails
    {
        bool IsValid(int type, int location);
        void InitAvailableTypes();
        void InitAvailableLocations();
    }
}
