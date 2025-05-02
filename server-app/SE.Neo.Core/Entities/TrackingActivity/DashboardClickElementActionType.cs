using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.TrackingActivity
{
    [Table("Dashboard_Click_Element_Action_Type")]
    public class DashboardClickElementActionType : BaseEntity
    {
        [Column("Dashboard_Click_Element_Action_Type_Id")]
        public Enums.DashboardClickElementActionType Id { get; set; }

        [Column("Dashboard_Click_Element_Action_Type_Name")]
        public string Name { get; set; }
    }
}
