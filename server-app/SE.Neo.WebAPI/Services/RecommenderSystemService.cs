using AutoMapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.WebAPI.Models.Project;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Net.Http.Headers;

namespace SE.Neo.WebAPI.Services
{
    public class RecommenderSystemService : IRecommenderSystemService
    {
        private readonly ILogger<RecommenderSystemService> _logger;
        private readonly IMapper _mapper;
        private readonly RecommenderSystemConfig _recommenderSystemConfig;
        private readonly HttpClient _httpClient;

        public RecommenderSystemService(ILogger<RecommenderSystemService> logger, IMapper mapper, IOptions<RecommenderSystemConfig> recommenderSystemConfig, HttpClient httpClient)
        {
            _logger = logger;
            _mapper = mapper;
            _recommenderSystemConfig = recommenderSystemConfig.Value;
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<int>> GetRecommendedProjectIds(int userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    _logger.LogInformation($"Started fetching recommended projects from new cluster for the userid: {userId}");

                    client.BaseAddress = new Uri(_recommenderSystemConfig.BaseConnectionUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var projectBaseUrl = _recommenderSystemConfig.ProjectApiUrl.Replace("user_id", Convert.ToString(userId));

                    _logger.LogInformation($"Connecting url for rec system: {projectBaseUrl}");
                    //GET Method
                    HttpResponseMessage response = await client.GetAsync(projectBaseUrl);

                    _logger.LogInformation($"Response status code recommended projects for the userid: {userId} is : {response.IsSuccessStatusCode}");

                    if (response.IsSuccessStatusCode)
                    {
                        var projectDetailsString = await response.Content.ReadAsStringAsync();//<List<ProjectBaserecommenderResponse>>();

                        _logger.LogInformation($"Response recommended projects for the userid: {userId} is : {projectDetailsString}");

                        var predictionsData = JsonConvert.DeserializeObject<ProjectRecommenderResponse>(projectDetailsString);

                        _logger.LogInformation($"Response recommended projects for the userid with predictions Data: {userId} is : {JsonConvert.SerializeObject(predictionsData)}");

                        if (predictionsData != null && predictionsData.Predictions.Any())
                        {
                            var predictionsList = predictionsData.Predictions.Select(x => x.Id);
                            _logger.LogInformation($"Response recommended projects for the userid with predictions Data: {userId} is : {JsonConvert.SerializeObject(predictionsList)}");

                            return predictionsList;
                        }

                    }
                    else
                    {
                        _logger.LogInformation($"Response recommended projects for the userid: {userId} for false query is : {JsonConvert.SerializeObject(response)}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while fetching recommended projects for the userid: {userId}. Exception: {ex.Message}");
            }
            return Enumerable.Empty<int>();
        }

    }
}
