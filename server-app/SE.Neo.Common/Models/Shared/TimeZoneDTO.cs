namespace SE.Neo.Common.Models.Shared
{
    public class TimeZoneDTO
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string StandardName { get; set; }

        public string DaylightName { get; set; }

        public string Abbreviation { get; set; }

        public string DaylightAbbreviation { get; set; }

        public string WindowsName { get; set; }

        public bool HasDST { get; set; }

        public double UTCOffset { get; set; }
    }
}
