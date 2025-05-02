using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Feedback")]
    public class Feedback : BaseIdEntity
    {
        [Column("Feedback_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Comments { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }


        public User User { get; set; }

    }
}
