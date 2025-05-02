using SE.Neo.WebAPI.Models.State;

namespace SE.Neo.WebAPI.Models.Country
{
    public class CountryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Code3 { get; set; }

        public IEnumerable<StateResponse> States { get; set; }
    }
}