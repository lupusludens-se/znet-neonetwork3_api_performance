namespace SE.Neo.Common.Models.Event
{
    public class EventModeratorDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Company { get; set; }

        public int? UserId { get; set; }

        public EventUserDTO? User { get; set; }
    }
}
