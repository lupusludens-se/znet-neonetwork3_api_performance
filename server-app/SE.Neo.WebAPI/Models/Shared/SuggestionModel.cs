using SE.Neo.Common.Enums;

namespace SE.Neo.WebAPI.Models.Shared
{
    public class SuggestionModel
    {
        public Guid Id { get; set; }

        public SuggestionType Type { get; set; }

        public string Name { get; set; }
    }
}