using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Models.Project
{
    public class BaseProjectOffsitePowerPurchaseAgreementDetailsResponse : BaseProjectDetailsResponse
    {
        public BaseProjectOffsitePowerPurchaseAgreementDetailsResponse() { }

        public BaseProjectOffsitePowerPurchaseAgreementDetailsResponse(
            BaseProjectOffsitePowerPurchaseAgreementDetailsResponse projectDetails)
        {
            this.Id = projectDetails.Id;
            this.ProjectId = projectDetails.ProjectId;
            this.Project = projectDetails.Project;
            this.Latitude = projectDetails.Latitude;
            this.Longitude = projectDetails.Longitude;
            this.IsExisting = projectDetails.IsExisting;
            this.IsoRtoId = projectDetails.IsoRtoId;
            this.IsoRtoName = projectDetails.IsoRtoName;
            this.ProductTypeId = projectDetails.ProductTypeId;
            this.ProductTypeName = projectDetails.ProductTypeName;
            this.CommercialOperationDate = projectDetails.CommercialOperationDate;
            this.ValuesToOfftakers = projectDetails.ValuesToOfftakers;
            this.PPATermYearsLength = projectDetails.PPATermYearsLength;
            this.TotalProjectNameplateMWACCapacity = projectDetails.TotalProjectNameplateMWACCapacity;
            this.TotalProjectExpectedAnnualMWhProductionP50 = projectDetails.TotalProjectExpectedAnnualMWhProductionP50;
            this.MinimumOfftakeMWhVolumeRequired = projectDetails.MinimumOfftakeMWhVolumeRequired;
            this.NotesForPotentialOfftakers = projectDetails.NotesForPotentialOfftakers;
        }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool IsExisting { get; set; }
        public int? IsoRtoId { get; set; }
        public string? IsoRtoName { get; set; }
        public int? ProductTypeId { get; set; }
        public string? ProductTypeName { get; set; }
        public DateTime? CommercialOperationDate { get; set; }
        public IEnumerable<BaseIdNameResponse> ValuesToOfftakers { get; set; }
        public int? PPATermYearsLength { get; set; }
        public int? TotalProjectNameplateMWACCapacity { get; set; }
        public int? TotalProjectExpectedAnnualMWhProductionP50 { get; set; }
        public float? MinimumOfftakeMWhVolumeRequired { get; set; }
        public string NotesForPotentialOfftakers { get; set; }
    }
}
