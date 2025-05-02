using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Event_Invited_Role")]
    [Index(nameof(EventId), nameof(RoleId), IsUnique = true)]
    public class EventInvitedRole : BaseIdEntity
    {
        [Column("Event_Invited_Role_Id")]
        public override int Id { get; set; }

        [Column("Role_Id")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public Role Role { get; set; }

        [Column("Event_Id")]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}
