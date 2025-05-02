using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.TrackingActivity
{
    [Table("Nav_Menu_Item")]
    public class NavMenuItem : BaseEntity
    {
        [Column("Nav_Menu_Item_Id")]
        public Enums.NavMenuItem Id { get; set; }

        [Column("Nav_Menu_Item_Name")]
        public string Name { get; set; }
    }
}
