using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;

namespace SE.Neo.Common.Models.Initiative
{
    /// <summary>
    /// Community User For Initiative
    /// </summary>
    public class CommunityUserForInitiativeDTO
    {

        /// <summary>
        /// Id of the User.
        /// </summary>
        public int Id { get; set; }
        public int InitiativeId { get; set; }
        public int TypeId { get; set; }

        public string JobTitle { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string? ImageName { get; set; }

        public BlobDTO? Image { get; set; }
        public int TagsTotalCount { get; set; }

        public IEnumerable<RoleDTO> Roles { get; set; }
        public IEnumerable<CategoryDTO> Categories { get; set; }
        public bool? IsNew { get; set; } = false;
    }
}
