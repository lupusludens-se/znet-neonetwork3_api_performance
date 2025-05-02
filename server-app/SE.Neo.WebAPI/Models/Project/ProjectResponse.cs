using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Company;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Opportunity { get; set; }
        public string Description { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime ChangedOn { get; set; }
        public int CategoryId { get; set; }
        public CategoryResponse Category { get; set; }
        public IEnumerable<TechnologyResponse> Technologies { get; set; }
        public int CompanyId { get; set; }
        public CompanyResponse Company { get; set; }
        public bool IsPinned { get; set; }
        public bool IsSaved { get; set; }
        public int OwnerId { get; set; }
        public UserResponse Owner { get; set; }
        public DateTime? PublishedOn { get; set; }
        public IEnumerable<RegionResponse> Regions { get; set; }
        public BaseProjectDetailsResponse ProjectDetails { get; set; }
        public DateTime? FirstTimePublishedOn { get; set; }
    }
}
