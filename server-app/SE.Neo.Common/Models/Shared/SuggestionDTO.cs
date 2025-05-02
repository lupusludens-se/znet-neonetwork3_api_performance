using SE.Neo.Common.Enums;

namespace SE.Neo.Common.Models.Shared
{
    public class SuggestionDTO
    {
        public Guid Id { get; set; }

        public SuggestionType Type { get; set; }

        public string Name { get; set; }
    }
}