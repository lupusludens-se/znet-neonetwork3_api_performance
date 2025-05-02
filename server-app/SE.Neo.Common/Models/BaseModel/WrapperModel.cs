namespace SE.Neo.Common.Models.Shared
{
    public class WrapperModel<T>
    {
        public IEnumerable<T> DataList { get; set; }
        public int Count { get; set; }
    }
}