using SE.Neo.Common.Models.Media;

namespace SE.Neo.Common.Models.Event
{
    public class EventUserDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int StatusId { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public bool IsFollowed { get; set; }

        public BlobDTO? Image { get; set; }

    }
}
