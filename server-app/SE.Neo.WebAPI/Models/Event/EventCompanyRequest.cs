using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventCompanyRequest
    {
        [CompanyIdExist]
        public int Id { get; set; }
    }
}
