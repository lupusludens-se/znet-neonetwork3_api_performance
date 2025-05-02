using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.CMS
{
    public class SolutionRequest
    {
        [SolutionIdExist]
        public int Id { get; set; }
    }
}