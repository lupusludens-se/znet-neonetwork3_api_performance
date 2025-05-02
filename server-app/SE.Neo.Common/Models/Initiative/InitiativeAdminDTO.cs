using SE.Neo.Common.Models.Company;

namespace SE.Neo.Common.Models.Initiative
{
    /// <summary>
    /// Initiatives for Admin view
    /// </summary>
    public class InitiativeAdminDTO : BaseInitiativeDTO
    {
        public string Phase { get; set; }

        public string StatusName { get; set; }

        public CompanyDTO Company { get; set; }

    }
}
