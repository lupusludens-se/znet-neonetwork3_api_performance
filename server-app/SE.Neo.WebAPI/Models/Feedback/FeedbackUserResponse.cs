using SE.Neo.Common.Models.Media;
using SE.Neo.WebAPI.Models.Role;

namespace SE.Neo.WebAPI.Models.Feedback
{
    public class FeedbackUserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StatusId { get; set; }
        public DateTime? RequestDeleteDate { get; set; }
        public string Company { get; set; }
        public string ImageName { get; set; }
        public BlobDTO Image { get; set; }
        public List<RoleResponse> Roles { get; set; }
    }
}
