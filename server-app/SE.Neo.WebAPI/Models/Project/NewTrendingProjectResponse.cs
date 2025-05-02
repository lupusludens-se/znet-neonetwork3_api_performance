namespace SE.Neo.WebAPI.Models.Project
{
    public class NewTrendingProjectResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string? CompanyImage { get; set; }
        public string? Tag { get; set; }
        public int ProjectCategoryId { get; set; }
        public string ProjectCategorySlug { get; set; }
        public string? ProjectCategoryImage { get; set; }
        public string? Geography { get; set; }
        public string? TrendingTag { get; set; }
        public List<string> Technologies { get; set; }
        public string? TechnologyImageSlug { get; set; }
    }
}
