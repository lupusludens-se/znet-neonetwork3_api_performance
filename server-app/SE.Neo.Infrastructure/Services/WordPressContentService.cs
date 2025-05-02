using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Extensions;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Constants;
using SE.Neo.Infrastructure.Models;
using SE.Neo.Infrastructure.Services.Interfaces;
using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SE.Neo.Infrastructure.Services
{
    public class WordPressContentService : IWordPressContentService
    {
        private readonly WordPressConfig _wordPressConnection;
        private readonly HttpClient _httpClient;
        private readonly IDistributedCache _cache;
        private readonly ILogger<WordPressContentService> _logger;

        public WordPressContentService(IOptions<WordPressConfig> wordPressConnection, IDistributedCache cache, HttpClient httpClient,
            ILogger<WordPressContentService> logger)
        {
            _cache = cache;
            _wordPressConnection = wordPressConnection.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> GetContentFromCMS(string paremeter)
        {

            var token = await FetchTokenAsync();

            var result = await GetAsync(_wordPressConnection.ConnectionBaseUrl + paremeter, token);

            return result;
        }

        public async Task<string> GetTaxonomyUpdateDate()
        {
            var token = await FetchTokenAsync();

            var result = await GetAsync(_wordPressConnection.ConnectionApiUrl + "taxonomy_update", token);

            return result;
        }

        private async Task<string> GetAsync(string url, string token)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.SendAsync(request);

                _logger.LogInformation($"wordpress token :{token} and url : {url} and response {response}");

                if (response != null)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return jsonString;
                }
                throw new Exception("Error occurred with response from CMS.");
            }
        }

        private async Task<string> FetchTokenAsync()
        {
            var token = await _cache.GetRecordAsync<CMSToken>(CacheKeys.CMSToken);

            if (token is null)
            {
                var tokenmodel = await GetTokenFromApi();

                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(
                        TimeSpan.FromSeconds(tokenmodel.expires_in));

                await _cache.SetRecordAsync(CacheKeys.CMSToken, tokenmodel, options);

                token = tokenmodel;
            }

            return token.jwt_token;
        }

        private async Task<CMSToken> GetTokenFromApi()
        {
            try
            {
                List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>> {
                    new KeyValuePair<string, string>("username", _wordPressConnection.WebApiUserName),
                    new KeyValuePair<string, string>("password", _wordPressConnection.WebApiUserPassword)
                };

                var content = new FormUrlEncodedContent(keyValuePairs);

                HttpResponseMessage response = await _httpClient.PostAsync(_wordPressConnection.ConnectionApiUrl + "token", content);

                var token = string.Empty;
                CMSToken tokenModel = new CMSToken();
                if (response.IsSuccessStatusCode)
                {
                    token = await response.Content.ReadAsStringAsync();

                    tokenModel = JsonSerializer.Deserialize<CMSToken>(token);
                }
                else
                {
                    throw new Exception("Error getting token for CMS.");
                }

                return tokenModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}