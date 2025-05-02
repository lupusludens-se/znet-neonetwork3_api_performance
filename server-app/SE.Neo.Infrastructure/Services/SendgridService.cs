using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SE.Neo.EmailTemplates.Models;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Services
{
    public class SendgridService : ISendgridService
    {
        private readonly IOptions<EmailConfig> _config;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SendgridService> _logger;

        public SendgridService(HttpClient httpClient, IOptions<EmailConfig> config,
            ILogger<SendgridService> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task<List<UndeliveredData>?> GetDataFromSendgrid(string undeliveredMailSubject)
        {
            try
            {
                var utcNow = DateTime.UtcNow;
                var previousDateUtcNow = DateTime.UtcNow.AddDays(-1);
                var start_date = new DateTime(previousDateUtcNow.Year, previousDateUtcNow.Month, previousDateUtcNow.Day, 0, 0, 0);
                var end_date = new DateTime(previousDateUtcNow.Year, previousDateUtcNow.Month, previousDateUtcNow.Day, 23, 59, 59);

                var formatted_start_date = string.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss.fff}Z", previousDateUtcNow, start_date);
                var formatted_end_date = string.Format("{0:yyyy-MM-dd}T{1:HH:mm:ss.fff}Z", previousDateUtcNow, end_date);
                var apirul = _config.Value.SendgridActivityAPI.Replace("_start_date_", formatted_start_date).Replace("_end_date_", formatted_end_date);
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(apirul),
                    Headers =
                        {
                            { "accept", "application/json" },
                            { "authorization", $"bearer {_config.Value.SendgridAPIKey}" },
                        }
                };
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<UndeliveredMailTemplatedModel>(body);
                    return result?.messages?.Where(x => !x.subject.StartsWith(undeliveredMailSubject))?.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while sending email activity. Error {ex.Message}. Detailed Error: {ex.InnerException?.Message}");
            }
            return default;
        }
    }
}