using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Common.Models.Public
{
    /// <summary>
    /// 
    /// </summary>
    public class DiscoverabilityProjectsDataDTO
    {
        [Column("Id")]
        public int Id { get; set; }

        public string ProjectType { get; set; }

        public string ProjectTypeSlug { get; set; }

        public string Technologies { get; set; }

        public string Geography { get; set; }
    }
}
