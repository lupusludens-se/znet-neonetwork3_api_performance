using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeAdminResponse
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string UserName { get; set; }
        public CategoryResponse Category { get; set; }
        public string CompanyName { get; set; }
        public string Phase { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime ModifiedOn { get; set; }

        public UserResponse User { get; set; }
        public IEnumerable<RegionResponse> Regions { get; set; }
    }
}
