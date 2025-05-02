using AutoMapper;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Project;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.ProjectDetails;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Models.Project;
using SE.Neo.WebAPI.Models.Project;

namespace SE.Neo.Core.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDTO>()
                .ForMember(dest => dest.StatusName, opts => opts.MapFrom(src => src.StatusId.ToString()))
                .ForMember(dest => dest.Technologies, opts => opts.MapFrom(src => src.Technologies.Select(o => o.Technology)))
                .ForMember(dest => dest.Regions, opts => opts.MapFrom(src => src.Regions.Select(o => o.Region)))
                .ForMember(
                    dest => dest.ProjectDetails,
                    opts => opts.MapFrom(src =>
                        (BaseProjectDetails)src.ProjectBatteryStorageDetails
                        ?? (BaseProjectDetails)src.ProjectCarbonOffsetsDetails
                        ?? (BaseProjectDetails)src.ProjectCommunitySolarDetails
                        ?? (BaseProjectDetails)src.ProjectEACDetails
                        ?? (BaseProjectDetails)src.ProjectEfficiencyAuditsAndConsultingDetails
                        ?? (BaseProjectDetails)src.ProjectEfficiencyEquipmentMeasuresDetails
                        ?? (BaseProjectDetails)src.ProjectEmergingTechnologyDetails
                        ?? (BaseProjectDetails)src.ProjectEVChargingDetails
                        ?? (BaseProjectDetails)src.ProjectFuelCellsDetails
                        ?? (BaseProjectDetails)src.ProjectGreenTariffsDetails
                        ?? (BaseProjectDetails)src.ProjectOffsitePowerPurchaseAgreementDetails
                        ?? (BaseProjectDetails)src.ProjectOnsiteSolarDetails
                        ?? (BaseProjectDetails)src.ProjectRenewableRetailDetails));

            CreateMap<ProjectDTO, Project>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Technologies, opts => opts.MapFrom(src => src.Technologies))
                .ForMember(dest => dest.Regions, opts => opts.MapFrom(src => src.Regions))
                .ForMember(dest => dest.Category, opts => opts.Ignore())
                .ForMember(dest => dest.CompanyId, opts => opts.Condition((src, dest, srcMember) => srcMember != default(int)))
                .ForMember(dest => dest.Company, opts => opts.Ignore())
                .ForMember(dest => dest.Owner, opts => opts.Ignore())
                .ForMember(dest => dest.FirstTimePublishedOn, opt => opt.Ignore());

            CreateMap<TechnologyDTO, ProjectTechnology>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TechnologyId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TechnologyId));

            CreateMap<BaseIdNameDTO, ProjectContractStructure>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ContractStructureId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContractStructureId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ContractStructureId.GetDescription()));

            CreateMap<BaseIdNameDTO, ProjectValueProvided>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ValueProvidedId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ValueProvidedId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ValueProvidedId.GetDescription()));

            CreateMap<BaseIdNameDTO, ProjectOffsitePPADetailsValueProvided>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ValueProvidedId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ValueProvidedId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ValueProvidedId.GetDescription()));

            CreateMap<BaseIdNameDTO, ProjectRenewableRetailDetailsPurchaseOption>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseOptionId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PurchaseOptionId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PurchaseOptionId.GetDescription()));

            CreateMap<BaseIdNameDTO, ProjectCarbonOffsetsDetailsTermLength>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TermLengthId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TermLengthId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TermLengthId.GetDescription()));

            CreateMap<BaseIdNameDTO, ProjectEACDetailsTermLength>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TermLengthId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TermLengthId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TermLengthId.GetDescription()));

            CreateMap<RegionDTO, ProjectRegion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RegionId));

            CreateMap<ProjectSaved, ProjectSavedDTO>();

            CreateMap<BaseProjectDetailsDTO, BaseProjectDetails>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ProjectId, opt => opt.Ignore())
                .ForMember(dest => dest.Project, opt => opt.Ignore());

            CreateMap<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>()
                .IncludeBase<BaseProjectDetailsDTO, BaseProjectDetails>();

            CreateMap<ProjectBatteryStorageDetailsDTO, ProjectBatteryStorageDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectFuelCellsDetailsDTO, ProjectFuelCellsDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectCarbonOffsetsDetailsDTO, ProjectCarbonOffsetsDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectCommunitySolarDetailsDTO, ProjectCommunitySolarDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectEACDetailsDTO, ProjectEACDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectEfficiencyAuditsAndConsultingDetailsDTO, ProjectEfficiencyAuditsAndConsultingDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectEfficiencyEquipmentMeasuresDetailsDTO, ProjectEfficiencyEquipmentMeasuresDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectEmergingTechnologyDetailsDTO, ProjectEmergingTechnologyDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectEVChargingDetailsDTO, ProjectEVChargingDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectGreenTariffsDetailsDTO, ProjectGreenTariffsDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectRenewableRetailDetailsDTO, ProjectRenewableRetailDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<ProjectOffsitePowerPurchaseAgreementDetailsDTO, ProjectOffsitePowerPurchaseAgreementDetails>()
                .IncludeBase<BaseProjectDetailsDTO, BaseProjectDetails>();

            CreateMap<ProjectOnsiteSolarDetailsDTO, ProjectOnsiteSolarDetails>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetails>();

            CreateMap<BaseProjectDetails, BaseProjectDetailsDTO>();

            CreateMap<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>()
                .IncludeBase<BaseProjectDetails, BaseProjectDetailsDTO>();

            CreateMap<ProjectBatteryStorageDetails, ProjectBatteryStorageDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectFuelCellsDetails, ProjectFuelCellsDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectCarbonOffsetsDetails, ProjectCarbonOffsetsDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectCommunitySolarDetails, ProjectCommunitySolarDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectEACDetails, ProjectEACDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectEfficiencyAuditsAndConsultingDetails, ProjectEfficiencyAuditsAndConsultingDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectEfficiencyEquipmentMeasuresDetails, ProjectEfficiencyEquipmentMeasuresDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectEmergingTechnologyDetails, ProjectEmergingTechnologyDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectEVChargingDetails, ProjectEVChargingDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectRenewableRetailDetails, ProjectRenewableRetailDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();

            CreateMap<ProjectGreenTariffsDetails, ProjectGreenTariffsDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>()
                .ForMember(dest => dest.TermLengthName, opts => opts.MapFrom(src => src.TermLengthId.HasValue ? src.TermLengthId.Value.GetDescription() : ""));

            CreateMap<ProjectOffsitePowerPurchaseAgreementDetails, ProjectOffsitePowerPurchaseAgreementDetailsDTO>()
                .ForMember(dest => dest.IsoRtoName, opts => opts.MapFrom(src => src.IsoRtoId != null
                    ? ((IsoRtoType)src.IsoRtoId).GetDescription()
                    : null))
                .ForMember(dest => dest.ProductTypeName, opts => opts.MapFrom(src => src.ProductTypeId != null
                    ? ((ProductType)src.ProductTypeId).GetDescription()
                    : null))
                .ForMember(dest => dest.SettlementTypeName, opts => opts.MapFrom(src => src.SettlementTypeId != null
                    ? ((SettlementType)src.SettlementTypeId).GetDescription()
                    : null))
                .ForMember(dest => dest.SettlementHubOrLoadZoneName, opts => opts.MapFrom(src => src.SettlementHubOrLoadZoneId != null
                    ? ((SettlementHubOrLoadZoneType)src.SettlementHubOrLoadZoneId).GetDescription()
                    : null))
                .ForMember(dest => dest.ForAllPriceEntriesCurrencyName, opts => opts.MapFrom(src => src.ForAllPriceEntriesCurrencyId != null
                    ? ((Enums.Currency)src.ForAllPriceEntriesCurrencyId).GetDescription()
                    : null))
                .ForMember(dest => dest.PricingStructureName, opts => opts.MapFrom(src => src.PricingStructureId != null
                    ? ((PricingStructureType)src.PricingStructureId).GetDescription()
                    : null))
                .ForMember(dest => dest.EACName, opts => opts.MapFrom(src => src.EACId != null
                    ? ((EACType)src.EACId).GetDescription()
                    : null))
                .ForMember(dest => dest.SettlementPriceIntervalName, opts => opts.MapFrom(src => src.SettlementPriceIntervalId != null
                    ? ((SettlementPriceIntervalType)src.SettlementPriceIntervalId).GetDescription()
                    : null))
                .ForMember(dest => dest.SettlementCalculationIntervalName, opts => opts.MapFrom(src => src.SettlementCalculationIntervalId != null
                    ? ((SettlementCalculationIntervalType)src.SettlementCalculationIntervalId).GetDescription()
                    : null))
                .IncludeBase<BaseProjectDetails, BaseProjectDetailsDTO>();

            CreateMap<ProjectOnsiteSolarDetails, ProjectOnsiteSolarDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetails, BaseCommentedProjectDetailsDTO>();
            CreateMap<ProjectDTO, SPDashboardProjectDetailsResponse>().ForMember(dest => dest.ProjectCategoryId, opt => opt.MapFrom(src => src.CategoryId)).
                ForMember(dest => dest.ProjectCategorySlug, opt => opt.MapFrom(src => src.Category.Slug)).ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusName))
                .ForMember(dest => dest.ChangedOn, opt => opt.MapFrom(src => src.ChangedOn.ToString("MM/dd/yy")));

            CreateMap<Project, ProjectResourceResponseDTO>()
                .ForMember(dest => dest.Technologies, opts => opts.MapFrom(src => src.Technologies.Select(o => o.Technology))) ;
        }

    }
}