using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Discoverability_Projects_Data")]
    public class DiscoverabilityProjectsData : BaseEntity
    {
        [Column("Id")]
        public int Id { get; set; }

        public string ProjectType { get; set; }

        public string Technologies { get; set; }

        public string Geography { get; set; }

        public string ProjectTypeSlug { get; set; }
    }
}
