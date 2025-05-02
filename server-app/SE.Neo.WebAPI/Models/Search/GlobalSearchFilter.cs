using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.WebAPI.Models.Search
{
    public class GlobalSearchFilter : BaseSearchFilterModel
    {
        public AzureSearchTaxonomyType? TaxonomyType { get; set; }

        public int? TaxonomyId { get; set; }

        public AzureSearchEntityType? EntityType { get; set; }
    }
}
