using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Email_Alert")]
    public class UserEmailAlert : BaseIdEntity
    {
        [Column("User_Email_Alert_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [Column("Email_Alert_Id")]
        [ForeignKey("EmailAlert")]
        public int EmailAlertId { get; set; }

        public EmailAlert EmailAlert { get; set; }

        [Column("Frequency")]
        public EmailAlertFrequency Frequency { get; set; }
    }
}