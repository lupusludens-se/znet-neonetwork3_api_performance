namespace SE.Neo.WebAPI.Models.Shared
{
    public class TimeZoneResponse
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }

        public string StandardName { get; set; }

        public bool HasDST { get; set; }

        public double UTCOffset { get; set; }
    }
}
