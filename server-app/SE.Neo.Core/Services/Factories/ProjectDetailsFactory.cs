using Microsoft.EntityFrameworkCore;
using SE.Neo.Common.Attributes;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Factories.Interfaces;
using SE.Neo.Core.Models.Project;

namespace SE.Neo.Core.Factories
{
    public class ProjectDetailsFactory : IProjectDetailsFactory
    {
        private readonly ApplicationContext _context;

        public ProjectDetailsFactory(ApplicationContext context)
        {
            _context = context;
        }

        private async Task<string> RetrieveCategorySlug(Project project)
        {
            return project.Category?.Slug ?? (await _context.Categories.FirstAsync(c => c.Id == project.CategoryId)).Slug;
        }

        public async Task<BaseProjectDetails> RemoveManyRelationsAsync(BaseProjectDetails baseProjectDetails)
        {
            string categorySlug;
            if (baseProjectDetails.Project == null)
            {
                Project project = await _context.Projects.Include(p => p.Category).FirstAsync(p => p.Id == baseProjectDetails.ProjectId);
                Category projectCategory = project.Category;
                categorySlug = projectCategory.Slug;
            }
            else
                categorySlug = await RetrieveCategorySlug(baseProjectDetails.Project);

            switch (categorySlug)
            {
                case CategoriesSlugs.AggregatedPowerPurchaseAgreement:
                case CategoriesSlugs.OffsitePowerPurchaseAgreement:
                    _context.RemoveRange(
                        _context.ProjectOffsitePPADetailsValuesProvided
                            .Where(pvp => pvp.ProjectOffsitePowerPurchaseAgreementDetailsId == baseProjectDetails.Id));
                    break;
                case CategoriesSlugs.CarbonOffsets:
                    _context.RemoveRange(
                        _context.ProjectCarbonOffsetsDetailsTermLengths
                            .Where(ptl => ptl.ProjectCarbonOffsetsDetailsId == baseProjectDetails.Id));
                    break;
                case CategoriesSlugs.EAC:
                    _context.RemoveRange(
                        _context.ProjectEACDetailsTermLengths
                            .Where(ptl => ptl.ProjectEACDetailsId == baseProjectDetails.Id));
                    break;
                case CategoriesSlugs.RenewableRetail:
                    _context.RemoveRange(
                        _context.ProjectRenewableRetailDetailsPurchaseOptions
                            .Where(ptl => ptl.ProjectRenewableRetailDetailsId == baseProjectDetails.Id));
                    break;
            }

            return baseProjectDetails;
        }

        public async Task<BaseProjectDetails> GetProjectDetailsAsync(Project project)
        {
            string categorySlug = await RetrieveCategorySlug(project);

            BaseProjectDetails? projectDetails = null;
            switch (categorySlug)
            {
                case CategoriesSlugs.BatteryStorage:
                    projectDetails = await _context.ProjectsBatteryStorageDetails.FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.FuelCells:
                    projectDetails = await _context.ProjectsFuelCellsDetails.FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.OnsiteSolar:
                    projectDetails = await _context.ProjectsOnsiteSolarDetails.FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.CommunitySolar:
                    projectDetails = await _context.ProjectsCommunitySolarDetails.FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.OffsitePowerPurchaseAgreement:
                case CategoriesSlugs.AggregatedPowerPurchaseAgreement:
                    projectDetails = await _context.ProjectsOffsitePowerPurchaseAgreementDetails.Include(p => p.ValuesToOfftakers).FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.CarbonOffsets:
                    projectDetails = await _context.ProjectsCarbonOffsetsDetails.Include(p => p.StripLengths).FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.EAC:
                    projectDetails = await _context.ProjectsEACDetails.Include(p => p.StripLengths).FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.EfficiencyAuditsAndConsulting:
                    projectDetails = await _context.ProjectsEfficiencyAuditsAndConsultingDetails.FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.EfficiencyEquipmentMeasures:
                    projectDetails = await _context.ProjectsEfficiencyEquipmentMeasuresDetails.FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.EmergingTechnology:
                    projectDetails = await _context.ProjectsEmergingTechnologyDetails.FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.EVCharging:
                    projectDetails = await _context.ProjectsEVChargingDetails.FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.GreenTariffs:
                    projectDetails = await _context.ProjectsGreenTariffsDetails.FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
                case CategoriesSlugs.RenewableRetail:
                    projectDetails = await _context.ProjectsRenewableRetailDetails.Include(p => p.PurchaseOptions).FirstOrDefaultAsync(p => p.ProjectId == project.Id);
                    break;
            }

            if (projectDetails == null)
                throw new CustomException($"{CoreErrorMessages.EntityNotFound} Project details not found.");

            return projectDetails;
        }
    }
}
