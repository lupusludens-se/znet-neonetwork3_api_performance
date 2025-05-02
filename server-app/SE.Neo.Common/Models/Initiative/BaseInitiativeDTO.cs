using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.User;

namespace SE.Neo.Common.Models.Initiative
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseInitiativeDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public UserDTO User { get; set; } = new UserDTO();

        public int ProjectTypeId { get; set; }

        [PropertyComparation("Scale")]
        public int ScaleId { get; set; }

        public int StatusId { get; set; }

        public int CurrentStepId { get; set; }

        public List<int> RegionIds { get; set; }
        public List<int> CollaboratorIds { get; set; }

        public CategoryDTO Category { get; set; }

        public IEnumerable<RegionDTO> Regions { get; set; }
        public IEnumerable<UserDTO> Collaborators { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? CreatedOn { get; set; }

    }
}
