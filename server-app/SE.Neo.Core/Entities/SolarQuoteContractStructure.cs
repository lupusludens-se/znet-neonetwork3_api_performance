using System.ComponentModel.DataAnnotations.Schema;


namespace SE.Neo.Core.Entities
{
    [Table("Solar_Quote_Contract_Structure")]
    public class SolarQuoteContractStructure : BaseIdEntity
    {
        [Column("Solar_Contract_Structure_Id")]
        public override int Id { get; set; }

        [Column("Solar_Quote_Id")]
        public int SolarQuoteId { get; set; }

        [Column("Contract_Structure_Id")]
        [ForeignKey("ContractStructure")]
        public Enums.ContractStructureType ContractStructureId { get; set; }

        public SolarQuote SolarQuote { get; set; }

        public ContractStructure ContractStructure { get; set; }
    }
}
