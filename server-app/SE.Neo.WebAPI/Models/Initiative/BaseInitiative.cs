using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class BaseInitiative
    {
        /// <summary>
        /// Initiative Id
        /// </summary>
        public int InitiativeId { get; set; }

        /// <summary>
        /// Initiative Title
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// Initiative Current Step Name
        /// </summary>
        public string CurrentStepName { get; set; }

        /// <summary>
        /// Initiative Categories
        /// </summary>
        public CategoryResponse Category { get; set; }

        public int ScaleId { get; set; }

        /// <summary>
        /// Initiative Regions
        /// </summary>
        public IEnumerable<RegionResponse> Regions { get; set; }

        /// <summary>
        /// Time of creation.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Time of Modification.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        public List<int>? CollaboratorIds { get; set; }

        /// <summary>
        /// Initiative Regions
        /// </summary>
        public IEnumerable<UserResponse> Collaborators { get; set; }
        /// <summary>
        /// Initiative Owner User Id
        /// </summary>
        public UserResponse? User { get; set; }

    }
}
