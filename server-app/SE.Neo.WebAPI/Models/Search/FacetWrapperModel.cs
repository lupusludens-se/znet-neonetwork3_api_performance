using SE.Neo.Common.Models.Shared;

namespace SE.Neo.WebAPI.Models.Search
{
    public class FacetWrapperModel<T> : WrapperModel<T>
    {
        public EntityTypeCountersModel Counters { get; set; }
    }
}
