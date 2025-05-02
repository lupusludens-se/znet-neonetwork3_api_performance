namespace SE.Neo.Common.Models.Company
{
    public class CompanyDomainDTO
    {
        public int Id { get; set; }

        public int CompanyId { get; set; }

        public string DomainName { get; set; }

        public bool IsActive { get; set; }
    }
}
