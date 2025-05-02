namespace SE.Neo.Common.Models.Shared
{
    public class TypeModel<T> : PaginationModel where T : struct
    {
        public T? Type { get; set; }
    }
}
