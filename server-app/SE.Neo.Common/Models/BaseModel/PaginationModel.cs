namespace SE.Neo.Common.Models
{
    public class PaginationModel
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public bool IncludeCount { get; set; }
    }
}
