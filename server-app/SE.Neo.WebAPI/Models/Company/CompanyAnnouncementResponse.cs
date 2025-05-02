using NetTopologySuite.Geometries;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Company
{
    public class CompanyAnnouncementResponse
    { 
        public string Title { get; set; }
        public string Link { get; set; }
         
        public int ScaleId { get; set; }
         
        public List<int> RegionIds { get; set; }

        public IEnumerable<RegionResponse> Regions { get; set; }

        public int Id { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
