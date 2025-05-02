using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.CMS;

namespace SE.Neo.Common.Models.Saved
{
    public class SavedContentItemDTO
    {
        public int Id { get; set; }

        public SavedContentType Type { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<CategoryDTO> Categories { get; set; }

        public IEnumerable<RegionDTO> Regions { get; set; }

        public IEnumerable<TechnologyDTO> Technologies { get; set; }

        public IEnumerable<SolutionDTO> Solutions { get; set; }
    }
}
