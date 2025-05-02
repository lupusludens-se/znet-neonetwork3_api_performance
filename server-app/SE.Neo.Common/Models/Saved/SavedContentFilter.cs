using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Saved
{
    public class SavedContentFilter : TypeModel<SavedContentType>
    {
        public string? Search { get; set; }
    }
}
