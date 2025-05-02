using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.TrackingActivity
{
    [Table("Activity_Type")]
    public class ActivityType : BaseEntity
    {
        [Column("Activitity_Type_Id")]
        public Enums.ActivityType Id { get; set; }

        [Column("Activitity_Type_Name")]
        public string Name { get; set; }
    }
}
