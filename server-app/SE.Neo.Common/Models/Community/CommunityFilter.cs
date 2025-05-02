namespace SE.Neo.Common.Models.Community
{
    public class CommunityFilter : PaginationModel
    {
        public string? Search { get; set; }
        public string? FilterBy { get; set; }
        public string? OrderBy { get; set; }
    }
}
