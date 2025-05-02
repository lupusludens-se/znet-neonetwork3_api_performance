using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Search
{
    public class SearchDocument
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public AzureSearchEntityType Type { get; set; }

        public List<BaseIdNameResponse> Categories { get; set; }

        public List<BaseIdNameResponse> Solutions { get; set; }

        public List<BaseIdNameResponse> Technologies { get; set; }

        public List<BaseIdNameResponse> Regions { get; set; }
        public List<BaseIdNameResponse> ContentTags { get; set; }
    }
}
