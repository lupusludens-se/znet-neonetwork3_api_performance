using SE.Neo.Common.Enums;

namespace SE.Neo.Core.Models.Saved
{
    public class SavedContentItem
    {
        public int Id { get; set; }

        public DateTime? CreatedOn { get; set; }

        public SavedContentType Type { get; set; }
    }
}
