using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Sub_Step")]
    public class InitiativeSubStep : BaseIdEntity
    {
        [Column("Initiative_Sub_Step_Id")]
        public override int Id { get; set; }

        [Column("Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Column("Initiative_Step_Id")]
        [ForeignKey("InitiativeStep")]
        public int StepId { get; set; }
        public InitiativeStep Step { get; set; }

        [Column("Title")]
        public string Title { get; set; }

        [Column("Content")]
        public string? Content { get; set; }


        [Column("Sub_Step_Order")]
        public int Order { get; set; }
    }
}
