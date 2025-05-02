using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum OffsitePPAs : int
    {
        [Description("Project Development")]
        ProjectDevelopment = 1,
        [Description("Ownership during Construction")]
        ConstructionOwnership,
        [Description("O&M Activities")]
        OMActivities,
        [Description("Long Term Ownership Interest")]
        OwnershipInterest,
        [Description("Technology Diversification")]
        TechnologyDiversification,
        [Description("Balancing Party")]
        BalancingParty,
        [Description("Retail / Integrated Company")]
        RetailCompany
    }
}
