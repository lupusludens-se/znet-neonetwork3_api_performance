using System.ComponentModel.DataAnnotations.Schema;


namespace SE.Neo.Core.Entities
{
    [Table("Solar_Quote_Interest")]
    public class SolarQuoteValueProvided : BaseIdEntity
    {
        [Column("Solar_Quote_Interest_Id")]
        public override int Id { get; set; }

        [Column("Solar_Quote_Id")]
        public int SolarQuoteId { get; set; }

        [Column("Value_Provided_Id")]
        [ForeignKey("ValueProvided")]
        public Enums.ValueProvidedType ValueProvidedId { get; set; }

        public SolarQuote SolarQuote { get; set; }

        public ValueProvided ValueProvided { get; set; }
    }
}
