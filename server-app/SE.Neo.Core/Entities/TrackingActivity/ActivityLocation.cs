using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.TrackingActivity
{
    [Table("Activity_Location")]
    public class ActivityLocation : BaseEntity
    {
        [Column("Activitity_Location_Id")]
        public Enums.ActivityLocation Id { get; set; }

        [Column("Activitity_Location_Name")]
        public string Name { get; set; }
    }
}
