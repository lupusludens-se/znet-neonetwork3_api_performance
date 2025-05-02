using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User")]
    [Index(nameof(FirstName), nameof(LastName))]
    [Index(nameof(Username), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class User : BaseIdEntity
    {
        [Key]
        [Column("User_Id")]
        public override int Id { get; set; }
        public virtual UserProfile UserProfile { get; set; }

        [Column("First_Name")]
        [MaxLength(64)]
        public string FirstName { get; set; }

        [Column("Last_Name")]
        [MaxLength(64)]
        public string LastName { get; set; }

        [Column("Username")]
        [MaxLength(70)] // AAD constraint
        public string Username { get; set; }

        [Column("Status_Id")]
        [ForeignKey("Status")]
        public Enums.UserStatus StatusId { get; set; }

        public virtual UserStatus Status { get; set; }

        [MaxLength(70)] // AAD constraint
        public string Email { get; set; }

        [Column("Company_Id")]
        [ForeignKey("Company")]
        public int CompanyId { get; set; }

        public Company Company { get; set; }

        [Column("Image_Name")]
        [ForeignKey("Image")]
        public string? ImageName { get; set; }

        public Blob? Image { get; set; }

        [Column("Time_Zone_Id")]
        [ForeignKey("TimeZone")]
        public int TimeZoneId { get; set; }

        public TimeZone TimeZone { get; set; }

        [Column("Country_Id")]
        [ForeignKey("Country")]
        public int CountryId { get; set; }

        public Country Country { get; set; }

        [Column("User_Heard_Via_Id")]
        [ForeignKey("HeardVia")]
        public Enums.HeardVia HeardViaId { get; set; }

        public HeardVia HeardVia { get; set; }

        [Column("Azure_Id")]
        [MaxLength(36)]
        public string AzureId { get; internal set; }


        [Column("Request_Delete_Date")]
        public DateTime? RequestDeleteDate { get; set; }

        public ICollection<UserRole> Roles { get; set; }

        public ICollection<UserPermission> Permissions { get; set; }

        public ICollection<UserNotification> Notifications { get; set; }

        public ICollection<UserFollower> FollowedUsers { get; set; }

        public ICollection<UserFollower> FollowerUsers { get; set; }

        public ICollection<UserEmailAlert> UserEmailAlerts { get; set; }

        /// <summary>
        /// Property is set to true or false in case of Admin or Internal SE users, otherwise its null.
        /// </summary>
        [Column("IsPrivateUser")]

        public bool? IsPrivateUser { get; set; }

        [Column("AdminComments")]
        [StringLength(12000)]
        public string? AdminComments { get; set; }
        /// <summary>
        /// Searched by Field
        /// </summary>
        [NotMapped]
        public int SearchedBy { get; set; }
        [NotMapped]
        public int StartsOrContains { get; set; }
    }
}
