using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Pending")]
    [Index(nameof(FirstName), nameof(LastName))]
    [Index(nameof(Email), IsUnique = true)]
    public class UserPending : BaseEntity
    {
        [Column("User_Pending_Id")]
        public int Id { get; set; }

        [Column("First_Name")]
        [MinLength(2), MaxLength(64)]
        public string FirstName { get; set; }

        [Column("Last_Name")]
        [MinLength(2), MaxLength(64)]
        public string LastName { get; set; }

        [MaxLength(70)] // AAD constraint
        public string Email { get; set; }

        public bool IsDenied { get; set; }

        [Column("User_Pending_Role_Id")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Column("Company_Name"), MaxLength(250)]
        public string CompanyName { get; set; }

        [Column("User_Pending_Company_Id")]
        [ForeignKey("Company")]
        public int? CompanyId { get; set; }

        public Company? Company { get; set; }

        [Column("User_Pending_Country_Id")]
        [ForeignKey("Country")]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        [Column("User_Pending_Time_Zone_Id")]
        [ForeignKey("TimeZone")]
        public int TimeZoneId { get; set; }

        public TimeZone TimeZone { get; set; }

        [Column("User_Pending_Heard_Via_Id")]
        [ForeignKey("HeardVia")]
        public Enums.HeardVia HeardViaId { get; set; }

        public HeardVia HeardVia { get; set; }

        [Column("AdminComments")]
        [StringLength(12000)]
        public string? AdminComments { get; set; }

        [Column("Joining_Interest_Details")]
        [StringLength(200)]
        public string? JoiningInterestDetails { get; set; }
    }
}
