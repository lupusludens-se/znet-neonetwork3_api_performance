using FluentValidation;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.Project;

namespace SE.Neo.WebAPI.Validators
{
    public class ProjectOffsitePowerPurchaseAgreementDetailsValidator : AbstractValidator<ProjectOffsitePowerPurchaseAgreementDetailsRequest>
    {
        public ProjectOffsitePowerPurchaseAgreementDetailsValidator()
        {
            RuleFor(x => x.ValuesToOfftakers)
                .NotEmpty()
                .When((x, d) => x.StatusId != ProjectStatus.Draft);

            RuleFor(x => x.UpsidePercentageToDeveloper)
                .NotNull()
                .When((x, d) => x.StatusId != ProjectStatus.Draft && x.PricingStructureId == PricingStructureType.UpsideShare);

            RuleFor(x => x.UpsidePercentageToOfftaker)
                .NotNull()
                .When((x, d) => x.StatusId != ProjectStatus.Draft && x.PricingStructureId == PricingStructureType.UpsideShare);

            RuleFor(x => x.DiscountAmount)
                .NotNull()
                .When((x, d) => x.StatusId != ProjectStatus.Draft && x.PricingStructureId == PricingStructureType.FixedDiscountToMarket);

            RuleFor(x => x.EACCustom)
                .NotNull()
                .When((x, d) => x.StatusId != ProjectStatus.Draft && x.EACId == EACType.Other);

            RuleFor(x => x.SettlementPriceIntervalCustom)
                .NotNull()
                .When((x, d) => x.StatusId != ProjectStatus.Draft && x.SettlementPriceIntervalId == SettlementPriceIntervalType.Other);
        }
    }
}
