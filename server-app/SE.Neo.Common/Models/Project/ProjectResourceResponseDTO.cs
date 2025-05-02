using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.CMS; 

namespace SE.Neo.Common.Models.Project
{ 
    public class ProjectResourceResponseDTO
    {

        [PropertyComparation("Project Type")]
        public int CategoryId { get; set; }

        public CategoryDTO Category { get; set; }

        [PropertyComparation("Technologies")]
        public IEnumerable<TechnologyDTO> Technologies { get; set; }

    }
}
