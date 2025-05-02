using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.CMS;

namespace SE.Neo.WebAPI.Models.Saved
{
    public class SavedContentItemResponse
    {
        public int Id { get; set; }

        public SavedContentType Type { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<CategoryResponse> Categories { get; set; }

        public IEnumerable<RegionResponse> Regions { get; set; }

        public IEnumerable<TechnologyResponse> Technologies { get; set; }

        public IEnumerable<SolutionResponse> Solutions { get; set; }
    }
}
