namespace SE.Neo.Common.Models.Activity
{
    public class ActivityDTO
    {
        public int Id { get; set; }

        public Guid SessionId { get; set; }

        public int TypeId { get; set; }

        public int LocationId { get; set; }

        public string Details { get; set; }
    }
}
