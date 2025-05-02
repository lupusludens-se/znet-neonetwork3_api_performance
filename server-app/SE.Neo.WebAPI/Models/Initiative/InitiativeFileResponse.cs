using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeFileResponse : BaseFile
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public int CreatedByUserId { get; set; }

        public int UpdatedByUserId { get; set; }

        public UserResponse CreatedByUser { get; set; }
        public UserResponse UpdatedByUser { get; set; }
    }
}
