namespace SE.Neo.Common.Models.Shared
{
    public class StateDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Abbr { get; set; }

        public int CountryId { get; set; }

        public CountryDTO Country { get; set; }
    }
}