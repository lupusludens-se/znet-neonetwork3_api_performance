using SE.Neo.Core.Constants;

namespace SE.Neo.WebAPI.Services
{
    public class BaseProjectApiService
    {
        private readonly ILogger<BaseProjectApiService> _logger;
        public BaseProjectApiService(ILogger<BaseProjectApiService> logger)
        {
            _logger = logger;
        }
        public string GetNextImageForProjectType(string projectType, Dictionary<string, List<string>> projectTypeToImages, Dictionary<string, int> projectTypeIndex)
        {
            try
            {
                List<string> images = projectTypeToImages[projectType];
                if (images == null || images.Count <= 0)
                {
                    return string.Empty;
                }

                int currentIndex = projectTypeIndex[projectType];
                string nextImage = images.ElementAt(currentIndex);
                currentIndex = (currentIndex + 1) % images.Count(); // Move to the next image in sequence.
                projectTypeIndex[projectType] = currentIndex; // Update the index for the project type.
                return nextImage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching the next image. Exception{ex.Message}. Detailed Message : {ex.InnerException?.Message}");
                return string.Empty;
            }
        }

        public string GetNextImageForProjectTechnology(string projectTechnology, Dictionary<string, List<string>> technologyTypeToImages, Dictionary<string, int> technologyTypeIndex)
        {
            try
            {
                List<string> images = technologyTypeToImages[projectTechnology];
                if (images == null || images.Count <= 0)
                {
                    return string.Empty;
                }

                int currentIndex = technologyTypeIndex[projectTechnology];
                string nextImage = images.ElementAt(currentIndex);
                currentIndex = (currentIndex + 1) % images.Count(); // Move to the next image in sequence.
                technologyTypeIndex[projectTechnology] = currentIndex; // Update the index for the project type.
                return nextImage;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching the next image. Exception{ex.Message}. Detailed Message : {ex.InnerException?.Message}");
                return string.Empty;
            }
        }

        public void InitializeProjectImages(List<string> projectCategories, Dictionary<string, List<string>> projectTypeToImages, Dictionary<string, int> projectTypeIndex)
        {
            GetProjectCategories(projectCategories);
            InitializeProjectTypeToImages(projectCategories, projectTypeToImages);
            InitializeProjectTypeIndex(projectCategories, projectTypeIndex);
        }
        public void InitializeProjectTechnologyImages(List<string> projectTechnologies, Dictionary<string, List<string>> projectTechnologyToImages, Dictionary<string, int> projectTechnologyTypeIndex)
        {
            GetProjectTechnologies(projectTechnologies);
            InitializeProjectTechnologyTypeToImages(projectTechnologies, projectTechnologyToImages);
            InitializeProjectTechnologyTypeIndex(projectTechnologies, projectTechnologyTypeIndex);
        }

        public void GetProjectCategories(List<string> projectCategories)
        {
            projectCategories.Add(CategoriesSlugs.AggregatedPowerPurchaseAgreement);
            projectCategories.Add(CategoriesSlugs.BatteryStorage);
            projectCategories.Add(CategoriesSlugs.CarbonOffsets);
            projectCategories.Add(CategoriesSlugs.CommunitySolar);
            projectCategories.Add(CategoriesSlugs.EAC);
            projectCategories.Add(CategoriesSlugs.EfficiencyAuditsAndConsulting);
            projectCategories.Add(CategoriesSlugs.EfficiencyEquipmentMeasures);
            projectCategories.Add(CategoriesSlugs.EmergingTechnology);
            projectCategories.Add(CategoriesSlugs.EVCharging);
            projectCategories.Add(CategoriesSlugs.FuelCells);
            projectCategories.Add(CategoriesSlugs.GreenTariffs);
            projectCategories.Add(CategoriesSlugs.MarketBrief);
            projectCategories.Add(CategoriesSlugs.OffsitePowerPurchaseAgreement);
            projectCategories.Add(CategoriesSlugs.OnsiteSolar);
            projectCategories.Add(CategoriesSlugs.RenewableRetail);
        }

        public void GetProjectTechnologies(List<string> projectTechnologies)
        {
            projectTechnologies.Add(TechnologiesSlugs.BatteryStorage);
            projectTechnologies.Add(TechnologiesSlugs.BuildingControls);
            projectTechnologies.Add(TechnologiesSlugs.BuildingEnvelope);
            projectTechnologies.Add(TechnologiesSlugs.CarportSolar);
            projectTechnologies.Add(TechnologiesSlugs.EmergingTechnology);
            projectTechnologies.Add(TechnologiesSlugs.EVCharging);
            projectTechnologies.Add(TechnologiesSlugs.FuelCells);
            projectTechnologies.Add(TechnologiesSlugs.GreenHydrogen);
            projectTechnologies.Add(TechnologiesSlugs.GroundmountSolar);
            projectTechnologies.Add(TechnologiesSlugs.HVAC);
            projectTechnologies.Add(TechnologiesSlugs.Lighting);
            projectTechnologies.Add(TechnologiesSlugs.OffshoreWind);
            projectTechnologies.Add(TechnologiesSlugs.OnshoreWind);
            projectTechnologies.Add(TechnologiesSlugs.RenewableThermal);
            projectTechnologies.Add(TechnologiesSlugs.RooftopSolar);
        }

        private void InitializeProjectTypeToImages(List<string> projectCategories, Dictionary<string, List<string>> projectTypeToImages)
        {
            foreach (string projectCategory in projectCategories)
            {
                projectTypeToImages.Add(projectCategory, new List<string>()
            {
                projectCategory + "-" + 1,
                projectCategory + "-" + 2,
                projectCategory + "-" + 3,
            });
            }

        }

        private void InitializeProjectTechnologyTypeToImages(List<string> projectTechnologies, Dictionary<string, List<string>> projectTechnologyTypeToImages)
        {
            foreach (string projectTechnology in projectTechnologies)
            {
                projectTechnologyTypeToImages.Add(projectTechnology, new List<string>()
            {
                projectTechnology + "-" + 1,
                projectTechnology + "-" + 2,
                projectTechnology + "-" + 3,
            });
            }

        }

        private void InitializeProjectTypeIndex(List<string> projectCategories, Dictionary<string, int> projectTypeIndex)
        {
            foreach (string projectCategory in projectCategories)
            {
                projectTypeIndex.Add(projectCategory, 0);
            }
        }

        private void InitializeProjectTechnologyTypeIndex(List<string> projectTechnologies, Dictionary<string, int> projectTechnologyTypeIndex)
        {
            foreach (string projectTechnology in projectTechnologies)
            {
                projectTechnologyTypeIndex.Add(projectTechnology, 0);
            }
        }
    }
}