using SE.Neo.Common.Models.CMS;

namespace SE.Neo.WebAPI.Models.Project
{
    public class SPDashboardProjectDetailsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int ProjectCategoryId { get; set; }
        public string ProjectCategorySlug { get; set; }
        public string? ProjectCategoryImage { get; set; }
        public string ChangedOn { get; set; }
        public IEnumerable<TechnologyDTO>? Technologies { get; set; }
    }
}
