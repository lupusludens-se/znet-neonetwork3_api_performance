using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Discussion")]
    public class InitiativeConversation : BaseIdEntity
    {
        [Column("Initiative_Discussion_Id")]
        public override int Id { get; set; }

        [Column("Initiative_Id")]
        [ForeignKey("Initiative")]
        public int InitiativeId { get; set; }

        public Initiative Initiative { get; set; }

        [Column("Discussion_Id")]
        [ForeignKey("Discussion")]
        public int DiscussionId { get; set; }

        public Discussion Discussion { get; set; }
    }
}
