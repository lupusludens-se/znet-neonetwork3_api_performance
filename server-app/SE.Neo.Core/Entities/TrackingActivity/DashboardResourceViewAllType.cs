using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.TrackingActivity
{
    [Table("Dashboard_Resource_View_All_Type")]
    public class DashboardResourceViewAllType : BaseEntity
    {
        [Column("Dashboard_Resource_View_All_Type_Id")]
        public Enums.DashboardResourceViewAllType Id { get; set; }

        [Column("Dashboard_Resource_View_All_Type_Name")]
        public string Name { get; set; }
    }
}
