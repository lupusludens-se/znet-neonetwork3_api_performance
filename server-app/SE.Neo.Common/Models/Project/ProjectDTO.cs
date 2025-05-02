using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectDTO
    {
        public int Id { get; set; }

        [PropertyComparation("Project Title")]
        public string Title { get; set; }

        [PropertyComparation("Sub-Title")]
        public string SubTitle { get; set; }

        [PropertyComparation("Describe the Opportunity")]
        public string Opportunity { get; set; }

        [PropertyComparation("About the Provider")]
        public string Description { get; set; }

        [PropertyComparation("Status")]
        public int StatusId { get; set; }

        public string StatusName { get; set; }


        public DateTime ChangedOn { get; set; }

        [PropertyComparation("Project Type")]
        public int CategoryId { get; set; }

        public CategoryDTO Category { get; set; }

        [PropertyComparation("Technologies")]
        public IEnumerable<TechnologyDTO> Technologies { get; set; }

        public int CompanyId { get; set; }

        public CompanyDTO Company { get; set; }

        public bool IsPinned { get; set; }

        public bool IsSaved { get; set; }

        public IEnumerable<ProjectSavedDTO> ProjectSaved { get; set; }

        [PropertyComparation("Owner")]
        public int OwnerId { get; set; }

        public UserDTO Owner { get; set; }

        public DateTime? PublishedOn { get; set; }
        public DateTime? FirstTimePublishedOn { get; set; }

        [PropertyComparation("Project tags")]
        public string Tags { get; set; }

        [PropertyComparation("Project Geography")]
        public IEnumerable<RegionDTO> Regions { get; set; }

        [PropertyComparation("Contract Structures")]
        public IEnumerable<BaseIdNameDTO> ContractStructures { get; set; }

        [PropertyComparation("Values Provided")]
        public IEnumerable<BaseIdNameDTO> ValuesProvided { get; set; }

        [PropertyComparation]
        public BaseProjectDetailsDTO ProjectDetails { get; set; }
    }
}
