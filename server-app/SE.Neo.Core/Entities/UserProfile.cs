using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Profile")]
    public class UserProfile : BaseEntity
    {
        [Key, ForeignKey("User")]
        [Column("User_Id")]
        public int UserId { get; set; }

        public virtual User User { get; set; }

        [Column("Job_Title")]
        [MaxLength(250)]
        public string JobTitle { get; set; }

        [Column("LinkedIn_Url")]
        [MaxLength(2048)]
        public string LinkedInUrl { get; set; }

        [Column("About")]
        public string About { get; set; }

        [Column("State_Id")]
        [ForeignKey("State")]
        public int? StateId { get; set; }

        public State? State { get; set; }

        [Column("Accept_Welcome_Series_Email")]
        public bool AcceptWelcomeSeriesEmail { get; set; }

        [Column("User_Responsibility_Id")]
        [ForeignKey("Responsibility")]
        public Enums.Responsibility? ResponsibilityId { get; set; }

        public Responsibility? Responsibility { get; set; }

        [Column("User_Login_Count")]
        public int? UserLoginCount { get; set; }

        public ICollection<UserProfileCategory> Categories { get; set; }

        public ICollection<UserProfileRegion> Regions { get; set; }

        public ICollection<UserProfileUrlLink> UrlLinks { get; set; }
        public ICollection<UserSkillsByCategory> SkillsByCategory { get; set; }
    }
}