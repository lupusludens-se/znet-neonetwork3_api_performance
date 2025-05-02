using SE.Neo.Core.Entities.CMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Collaborator")]
    public class InitiativeCollaborator : BaseIdEntity
    {
        [Column("Initiative_Collaborator_Id")]
        public override int Id { get; set; }

        [Column("Initiative_Id")]
        [ForeignKey("Initiative")]
        public int InitiativeId { get; set; }
        public Initiative Initiative { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
