using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SE.Neo.Core.Entities
{
    [Table("Solar_Quote")]
    public class SolarQuote : BaseEntity
    {
        public int Id { get; set; }

        [Column("Site_Address")]
        [MaxLength(250)]
        public string? SiteAddress { get; set; }

        [Column("Annual_Power")]
        public long? AnnualPower { get; set; }

        [Column("Building_Owned")]
        public bool? BuildingOwned { get; set; }

        [Column("Roof_Available")]
        public bool? RoofAvailable { get; set; }

        [Column("Roof_Area")]
        public long? RoofArea { get; set; }

        [Column("Roof_Area_Type")]
        public AreaType? RoofAreaType { get; set; }

        [Column("Land_Available")]
        public bool? LandAvailable { get; set; }

        [Column("Land_Area")]
        public long? LandArea { get; set; }

        [Column("Land_Area_Type")]
        public AreaType? LandAreaType { get; set; }

        [Column("Carport_Available")]
        public bool? CarportAvailable { get; set; }

        [Column("Carport_Area")]
        public long? CarportArea { get; set; }

        [Column("Carport_Area_Type")]
        public AreaType? CarportAreaType { get; set; }

        public ICollection<SolarQuoteContractStructure> ContractStructures { get; set; }

        public ICollection<SolarQuoteValueProvided> Interests { get; set; }

        [Column("Additional_Comments")]
        [MaxLength(4000)]
        public string? AdditionalComments { get; set; }

        [Column("Portfolio_Review")]
        public bool PortfolioReview { get; set; }

        [Column("Requested_By_User_Id")]
        [ForeignKey("RequestedByUser")]
        public int RequestedByUserId { get; set; }

        public User RequestedByUser { get; set; }


    }
}
