using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Job_Log")]
    public class JobLog : BaseIdEntity
    {
        [Column("Job_Log_Id")]
        public override int Id { get; set; }

        [Column("Last_Run_Time")]
        public DateTime LastRunTime { get; set; }

        public bool Success { get; set; }

        [Column("Job_Type")]
        public JobType JobType { get; set; }
    }
}
