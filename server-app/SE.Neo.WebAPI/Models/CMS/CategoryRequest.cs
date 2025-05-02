using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.CMS
{
    public class CategoryRequest
    {
        [CategoryIdExist]
        public int Id { get; set; }
    }
}