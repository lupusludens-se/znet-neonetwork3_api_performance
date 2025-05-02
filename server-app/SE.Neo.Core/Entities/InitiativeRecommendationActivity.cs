using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Recommendation_Activity")]
    public class InitiativeRecommendationActivity : BaseEntity
    {
        [Column("Initiative_Recommendation_Activity_Id")]
        public int Id { get; set; }

        [Column("Initiative_Id")]
        [ForeignKey("Initiative")]
        public int InitiativeId { get; set; }

        [Column("Article_Last_Viewed_Date")]
        public DateTime ArticleLastViewedDate { get; set; }

        [Column("Projects_Last_Viewed_Date")]
        public DateTime ProjectsLastViewedDate { get; set; }

        [Column("Tools_Last_Viewed_Date")]
        public DateTime ToolsLastViewedDate { get; set; }

        [Column("Community_Last_Viewed_Date")]
        public DateTime CommunityLastViewedDate { get; set; }
        public Initiative Initiative { get; set; }
    }
}
