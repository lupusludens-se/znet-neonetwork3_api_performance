using SE.Neo.Common.Models.Media;

namespace SE.Neo.Common.Models.Initiative
{
    public class ConversationUserForInitiativeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public string Company { get; set; }
        public string ImageName { get; set; }
        public BlobDTO Image { get; set; }
    }
}
