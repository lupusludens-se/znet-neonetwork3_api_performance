﻿using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.TrackingActivity
{
    [Table("Activity")]
    public class Activity : BaseIdEntity
    {
        [Column("Activitity_Id")]
        public override int Id { get; set; }

        [Column("Session_Id")]
        public Guid SessionId { get; set; }

        [Column("Activity_Type_Id")]
        [ForeignKey("Type")]
        public Enums.ActivityType TypeId { get; set; }

        public ActivityType Type { get; set; }

        [Column("Activity_Location_Id")]
        [ForeignKey("Location")]
        public Enums.ActivityLocation LocationId { get; set; }

        public ActivityLocation Location { get; set; }

        // JSON object depends on Type
        public string Details { get; set; }
    }
}
