using Microsoft.EntityFrameworkCore;
using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Company")]
    [Index(nameof(Name))]
    public class Company : BaseIdEntity
    {
        [Column("Company_Id")]
        public override int Id { get; set; }

        [Column("Company_Name")]
        [MaxLength(250)]
        public string Name { get; set; }

        [Column("Status_Id")]
        [ForeignKey("Status")]
        public Enums.CompanyStatus StatusId { get; set; }

        public virtual CompanyStatus Status { get; set; }

        [Column("Type_Id")]
        public CompanyType TypeId { get; set; }

        [Column("Image_Logo")]
        [ForeignKey("Image")]
        public string? ImageLogo { get; set; }

        public Blob? Image { get; set; }

        [Column("Company_Url")]
        [MaxLength(2048)]
        public string CompanyUrl { get; set; }

        [Column("LinkedIn_Url")]
        [MaxLength(2048)]
        public string LinkedInUrl { get; set; }

        [Column("About")]
        [StringLength(12000)]
        public string About { get; set; }

        [Column("Country_Id")]
        [ForeignKey("Country")]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        [Column("MDM_Key")]
        [MaxLength(14)]
        public string MDMKey { get; set; }

        [Column("Industry_Id")]
        [ForeignKey("Industry")]
        public int IndustryId { get; set; }

        public Industry Industry { get; set; }

        public ICollection<CompanyUrlLink> UrlLinks { get; set; }

        public ICollection<Project> Projects { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<CompanyCategory> Categories { get; set; }

        public ICollection<CompanyOffsitePPA> OffsitePPAs { get; set; }
        [NotMapped]
        public int SearchedBy { get; set; }

        [NotMapped]
        public int StartsOrContains { get; set; }

    }
}