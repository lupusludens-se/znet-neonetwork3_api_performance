using AutoMapper;
using SE.Neo.Common.Models.Project;
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Project;
using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Mapping
{
    public class ProjectApiProfile : Profile
    {
        public ProjectApiProfile()
        {
            CreateMap<EnumRequest<ProjectBatteryStorageContractStructureType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectBatteryStorageValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectCarbonOffsetsValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectCommunitySolarContractStructureType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectCommunitySolarValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectEACValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectEfficiencyAuditsAndConsultingContractStructureType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectEfficiencyAuditsAndConsultingValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectEfficiencyEquipmentMeasuresContractStructureType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectEfficiencyEquipmentMeasuresValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectEmergingTechnologyContractStructureType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectEmergingTechnologyValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectEVChargingContractStructureType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectEVChargingValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectFuelCellsContractStructureType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectFuelCellsValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectGreenTariffsValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectOffsitePPAValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectOnsiteSolarContractStructureType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectOnsiteSolarValueProvidedType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<ProjectRenewableRetailValueProvidedType>, BaseIdNameDTO>();

            CreateMap<BaseProjectDetailsRequest, BaseProjectDetailsDTO>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .IncludeBase<BaseProjectDetailsRequest, BaseProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectBatteryStorageDetailsRequest, ProjectBatteryStorageDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectFuelCellsDetailsRequest, ProjectFuelCellsDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectOffsitePowerPurchaseAgreementDetailsRequest, ProjectOffsitePowerPurchaseAgreementDetailsDTO>()
                .IncludeBase<BaseProjectDetailsRequest, BaseProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectCarbonOffsetsDetailsRequest, ProjectCarbonOffsetsDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectCommunitySolarDetailsRequest, ProjectCommunitySolarDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectEACDetailsRequest, ProjectEACDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectEfficiencyAuditsAndConsultingDetailsRequest, ProjectEfficiencyAuditsAndConsultingDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectEfficiencyEquipmentMeasuresDetailsRequest, ProjectEfficiencyEquipmentMeasuresDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectEmergingTechnologyDetailsRequest, ProjectEmergingTechnologyDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectEVChargingDetailsRequest, ProjectEVChargingDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectGreenTariffsDetailsRequest, ProjectGreenTariffsDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectOnsiteSolarDetailsRequest, ProjectOnsiteSolarDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectRenewableRetailDetailsRequest, ProjectRenewableRetailDetailsDTO>()
                .IncludeBase<BaseCommentedProjectDetailsRequest, BaseCommentedProjectDetailsDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<ProjectBatteryStorageDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectBatteryStorageDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectFuelCellsDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectFuelCellsDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectCarbonOffsetsDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectCarbonOffsetsDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided));

            CreateMap<ProjectCommunitySolarDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectCommunitySolarDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectEACDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectEACDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided));

            CreateMap<ProjectEfficiencyAuditsAndConsultingDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectEfficiencyAuditsAndConsultingDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectEfficiencyEquipmentMeasuresDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectEfficiencyEquipmentMeasuresDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectEmergingTechnologyDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectEmergingTechnologyDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectEVChargingDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectEVChargingDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectGreenTariffsDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectGreenTariffsDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided));

            CreateMap<ProjectOnsiteSolarDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectOnsiteSolarDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectRenewableRetailDetailsRequest, ProjectDTO>()
                .IncludeBase<BaseProjectDetailsRequest, ProjectDTO>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectRenewableRetailDetailsRequest, ProjectDTO>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided));

            CreateMap<BaseProjectDetailsDTO, BaseProjectDetailsResponse>();

            CreateMap<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>()
                .IncludeBase<BaseProjectDetailsDTO, BaseProjectDetailsResponse>();

            CreateMap<ProjectBatteryStorageDetailsDTO, ProjectBatteryStorageDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectFuelCellsDetailsDTO, ProjectFuelCellsDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectOffsitePowerPurchaseAgreementDetailsDTO, ProjectOffsitePowerPurchaseAgreementDetailsResponse>()
                .IncludeBase<BaseProjectDetailsDTO, BaseProjectDetailsResponse>();

            CreateMap<ProjectCarbonOffsetsDetailsDTO, ProjectCarbonOffsetsDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectCommunitySolarDetailsDTO, ProjectCommunitySolarDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectEACDetailsDTO, ProjectEACDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectEfficiencyAuditsAndConsultingDetailsDTO, ProjectEfficiencyAuditsAndConsultingDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectEfficiencyEquipmentMeasuresDetailsDTO, ProjectEfficiencyEquipmentMeasuresDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectEmergingTechnologyDetailsDTO, ProjectEmergingTechnologyDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectEVChargingDetailsDTO, ProjectEVChargingDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectGreenTariffsDetailsDTO, ProjectGreenTariffsDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectOnsiteSolarDetailsDTO, ProjectOnsiteSolarDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectRenewableRetailDetailsDTO, ProjectRenewableRetailDetailsResponse>()
                .IncludeBase<BaseCommentedProjectDetailsDTO, BaseCommentedProjectDetailsResponse>();

            CreateMap<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());

            CreateMap<ProjectDTO, ProjectBatteryStorageDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectBatteryStorageDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectDTO, ProjectFuelCellsDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectFuelCellsDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectDTO, ProjectCarbonOffsetsDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectCarbonOffsetsDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided));

            CreateMap<ProjectDTO, ProjectCommunitySolarDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectCommunitySolarDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectDTO, ProjectEACDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectEACDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided));

            CreateMap<ProjectDTO, ProjectEfficiencyAuditsAndConsultingDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectEfficiencyAuditsAndConsultingDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectDTO, ProjectEfficiencyEquipmentMeasuresDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectEfficiencyEquipmentMeasuresDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectDTO, ProjectEmergingTechnologyDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectEmergingTechnologyDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectDTO, ProjectEVChargingDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectEVChargingDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectDTO, ProjectGreenTariffsDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectGreenTariffsDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided));

            CreateMap<ProjectDTO, ProjectOnsiteSolarDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectOnsiteSolarDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided))
                .ForMember(dest => dest.ContractStructures, opts => opts.MapFrom(src => src.ContractStructures));

            CreateMap<ProjectDTO, ProjectRenewableRetailDetailsResponse>()
                .IncludeBase<ProjectDTO, BaseProjectDetailsResponse>()
                .ForAllMembers(opts => opts.Ignore());
            CreateMap<ProjectDTO, ProjectRenewableRetailDetailsResponse>()
                .ForMember(dest => dest.ValuesProvided, opts => opts.MapFrom(src => src.ValuesProvided));

            CreateMap<ProjectRequest, ProjectDTO>()
                .AddTransform<string>(s => s == null ? string.Empty : s)
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<ProjectDTO, ProjectResponse>();
            CreateMap<ProjectResponse, ProjectDTO>();
            CreateMap<ProjectDTO, RecentProjectResponse>();

            CreateMap<ProjectResourceResponseDTO, ProjectResourceResponse>();

            CreateMap<ProjectDTO, SPCompanyProjectResponse>()
                 .ForMember(dest => dest.ProjectCategorySlug, opt => opt.MapFrom(src => src.Category.Slug));
        }
    }
}