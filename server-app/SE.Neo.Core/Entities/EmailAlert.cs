using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Email_Alert")]
    public class EmailAlert : BaseIdEntity
    {
        [Column("Email_Alert_Id")]
        public override int Id { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("Category")]
        public EmailAlertCategory Category { get; set; }

        [Column("Frequency")]
        public EmailAlertFrequency Frequency { get; set; }

        public ICollection<UserEmailAlert> UserEmailAlerts { get; set; }
    }
}