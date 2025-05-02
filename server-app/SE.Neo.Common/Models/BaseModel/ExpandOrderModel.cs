namespace SE.Neo.Common.Models.Shared
{
    public class ExpandOrderModel : PaginationModel
    {
        public string? Expand { get; set; }

        public string? OrderBy { get; set; }
    }

}
