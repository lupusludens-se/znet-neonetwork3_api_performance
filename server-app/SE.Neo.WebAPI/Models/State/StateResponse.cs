using SE.Neo.WebAPI.Models.Country;

namespace SE.Neo.WebAPI.Models.State
{
    public class StateResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbr { get; set; }

        public int CountryId { get; set; }

        public CountryResponse Country { get; set; }
    }
}