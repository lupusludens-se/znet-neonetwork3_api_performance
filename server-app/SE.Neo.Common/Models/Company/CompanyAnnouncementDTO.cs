using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.User;

namespace SE.Neo.Common.Models.Company
{
    public class CompanyAnnouncementDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        //public UserDTO User { get; set; } = new UserDTO();


        public string Link { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        [PropertyComparation("Scale")]
        public int ScaleId { get; set; }

        public int StatusId { get; set; }

        public List<int> RegionIds { get; set; }

        public IEnumerable<RegionDTO> Regions { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
 