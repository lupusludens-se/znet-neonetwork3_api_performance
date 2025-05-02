using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.CMS
{
    public class TechnologyRequest
    {
        [TechnologyIdExist]
        public int Id { get; set; }
    }
}