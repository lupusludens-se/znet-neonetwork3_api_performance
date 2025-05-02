using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative")]
    public class Initiative : BaseIdEntity
    {
        [Column("Initiative_Id")]
        public override int Id { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Project_Type_Id")]
        [ForeignKey("ProjectType")]
        public int ProjectTypeId { get; set; }
        public Category ProjectType { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [Column("Status_Id")]
        [ForeignKey("Initiative_Status")]
        public Enums.InitiativeStatus StatusId { get; set; }
        public virtual InitiativeStatus Status { get; set; }
        [Column("Scale_Id")]
        [ForeignKey("Initiative_Scale")]
        public int ScaleId { get; set; }

        [Column("Current_Step_Id")]
        [ForeignKey("InitiativeStep")]
        public int CurrentStepId { get; set; }
        public InitiativeStep InitiativeStep { get; set; }


        public ICollection<InitiativeArticle> Articles { get; set; }

        public ICollection<InitiativeCommunity> Users { get; set; }

        public ICollection<InitiativeConversation> Conversations { get; set; }

        public ICollection<InitiativeProject> Projects { get; set; }

        public ICollection<InitiativeTool> Tools { get; set; }

        public ICollection<InitiativeRegion> Regions { get; set; }

        public ICollection<InitiativeProgressDetails> ProgressDetails { get; set; }

        public ICollection<InitiativeFile> Files { get; set; }

        public ICollection<InitiativeCollaborator> Collaborators { get; set; }
    }
}