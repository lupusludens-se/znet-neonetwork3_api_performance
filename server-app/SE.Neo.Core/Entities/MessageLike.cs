using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Message_Like")]
    public class MessageLike : BaseIdEntity
    {
        [Column("Message_Like_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("Message_Id")]
        [ForeignKey("Message")]
        public int MessageId { get; set; }

        public User User { get; set; }

        public Message Message { get; set; }
    }
}
