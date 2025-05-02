namespace SE.Neo.Common.Models.Shared
{
    public class CountryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string Code3 { get; set; }

        public IEnumerable<StateDTO> States { get; set; }
    }
}