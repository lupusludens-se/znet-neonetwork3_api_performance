using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Company;


namespace SE.Neo.Common.Models.Initiative
{
    public class ProjectForInitiativeDTO
    {
        public int Id { get; set; }
        public int InitiativeId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public CategoryDTO Category { get; set; }
        public CompanyDTO Company { get; set; }
        public IEnumerable<RegionDTO> Regions { get; set; }
        public bool? IsNew{ get; set; }
    }
}
