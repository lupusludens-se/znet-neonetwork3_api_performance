using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.CMS
{
    public class RegionRequest
    {
        [RegionIdExist]
        public int Id { get; set; }
    }
}