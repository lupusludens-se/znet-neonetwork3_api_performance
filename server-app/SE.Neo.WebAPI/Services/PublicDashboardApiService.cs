using AutoMapper;
using SE.Neo.Common;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Project;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class PublicDashboardApiService : BaseProjectApiService, IPublicDashboardApiService
    {
        private readonly ILogger<PublicDashboardApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IPublicDashboardService _publicDashboardService;
        public PublicDashboardApiService(
            ILogger<PublicDashboardApiService> logger,
            IMapper mapper,
            IPublicDashboardService publicDashboardService) : base(logger)
        {
            _logger = logger;
            _publicDashboardService = publicDashboardService;
            _mapper = mapper;
        }

        public async Task<WrapperModel<NewTrendingProjectResponse>> GetProjectsDataForDiscoverability()
        {
            Dictionary<string, List<string>> projectTypeToImages = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> technologyTypeToImages = new Dictionary<string, List<string>>();

            Dictionary<string, int> projectTypeIndex = new Dictionary<string, int>();
            Dictionary<string, int> technologyTypeIndex = new Dictionary<string, int>();

            List<string> projectCategories = new List<string>();
            List<string> projectTechnologies = new List<string>();

            InitializeProjectImages(projectCategories, projectTypeToImages, projectTypeIndex);
            InitializeProjectTechnologyImages(projectTechnologies, technologyTypeToImages, technologyTypeIndex);
            var disProjectsDataDTOs = await _publicDashboardService.GetProjectsDataForDiscoverability();

            List<NewTrendingProjectResponse> result = disProjectsDataDTOs.Select(item => new NewTrendingProjectResponse()
            {
                Id = item.Id,
                Title = item.ProjectType,
                SubTitle = item.Technologies,
                Geography = item.Geography,
                ProjectCategorySlug = item.ProjectTypeSlug,
                Technologies = item.Technologies.Split(',').ToList()
            }).ToList();


            var finalResult = GetRandomRecords(result);

            for (int i = 0; i < finalResult.Count; i++)
            {
                NewTrendingProjectResponse? item = finalResult[i];
                if(item.Technologies.Count > 0)
                {
                    List<string> technologySplit = item.Technologies[0].Split(' ').ToList();
                    string technologyType = string.Join("", technologySplit);
                    string technologySlug = TechnologiesSlugs.GetSlugValue(technologyType);
                    item.TechnologyImageSlug = technologySlug;
                    item.ProjectCategoryImage = GetNextImageForProjectType(technologySlug, technologyTypeToImages, technologyTypeIndex);
                }
                else
                {
                    item.ProjectCategoryImage = GetNextImageForProjectType(item.ProjectCategorySlug, projectTypeToImages, projectTypeIndex);
                }
                
                item.TrendingTag = i <= 6 ? ((i % 2 == 0) ? ZnConstants.Trending : ZnConstants.New) : ZnConstants.Trending;
            }

            var wrapper = new WrapperModel<NewTrendingProjectResponse>
            {
                Count = result.Count,
                DataList = finalResult
            };
            return wrapper;
        }

        private List<NewTrendingProjectResponse> GetRandomRecords(List<NewTrendingProjectResponse> data)
        {
            var result = new List<NewTrendingProjectResponse>();
            var random = new Random();

            data = data.OrderBy(_ => random.Next()).ToList();

            var uniqueProjectTypes = data.Select(rec => rec.Title).Distinct().Take(ZnConstants.TotalNewTrendingProjectsCountInDashboard).ToList();

            foreach (var item in uniqueProjectTypes)
            {
                var record = data.FirstOrDefault(r => r.Title == item);
                result.Add(record);
            }

            var remainingRecords = data.Except(result).ToList();
            for (int i = 0; i < (ZnConstants.TotalNewTrendingProjectsCountInDashboard - uniqueProjectTypes.Count); i++)
            {
                var index = random.Next(0, remainingRecords.Count);
                result.Add(remainingRecords[index]);
            }

            while (HasAdjacentProjectTypes(result))
            {
                result = ShuffleList(result);
            }
            return result;
        }

        private List<NewTrendingProjectResponse> ShuffleList(List<NewTrendingProjectResponse> results)
        {
            Random _random = new Random();
            List<NewTrendingProjectResponse> shuffledResults = new List<NewTrendingProjectResponse>();
            while (results.Count > 0)
            {
                int index = _random.Next(0, results.Count);
                shuffledResults.Add(results[index]);
                results.RemoveAt(index);
            }

            return shuffledResults;
        }

        private bool HasAdjacentProjectTypes(List<NewTrendingProjectResponse> results)
        {
            for (int i = 0; i < results.Count - 1; i++)
            {
                return results[i].Title == results[i + 1].Title;
            }
            return false;
        }
    }
}