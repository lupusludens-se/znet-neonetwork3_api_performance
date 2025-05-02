using SE.Neo.Core.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    public class BaseEntity : ITimeStamp
    {
        [Column("Created_User_Id")]
        public int? CreatedByUserId { get; set; }

        [Column("Updated_User_Id")]
        public int? UpdatedByUserId { get; set; }

        [Column("Created_Ts")]
        public DateTime? CreatedOn { get; set; }

        [Column("Last_Change_Ts")]
        public DateTime? ModifiedOn { get; set; }
    }
}