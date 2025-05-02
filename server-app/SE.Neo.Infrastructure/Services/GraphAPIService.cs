using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SE.Neo.Common.Extensions;
using SE.Neo.Core.Configs;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Constants;
using SE.Neo.Infrastructure.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;

namespace SE.Neo.Infrastructure.Services
{
    public class GraphAPIService : IGraphAPIService
    {
        private readonly GraphAPIConfig _graphAPIConfig;
        private readonly ILogger<GraphAPIService> _logger;
        private readonly HttpClient _httpClient;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;
        private readonly IDistributedCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;

        private const string BaseUrl = "https://graph.microsoft.com/v1.0";

        public GraphAPIService(
            IOptions<GraphAPIConfig> graphAPIConfig,
            ILogger<GraphAPIService> logger,
            HttpClient httpClient,
            IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp,
            IDistributedCache cache,
            IHttpClientFactory httpClientFactory)
        {
            _graphAPIConfig = graphAPIConfig.Value;
            _logger = logger;
            _httpClient = httpClient;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
            _cache = cache;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> AddUserAndResetPasswordAsync(string firstName, string lastName, string email, bool retry = true)
        {
            _logger.LogInformation("Add User and Reset Password: Getting token...");
            await EnrichWithTokenAsync();

            string newUserId;
            _logger.LogInformation("Add User and Reset Password: Adding user...");

            try
            {
                newUserId = await AddUserAsync(firstName, lastName, email);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Something went wrong during adding a user to Azure. Trying to add the user again...");

                if (retry)
                    return await AddUserAndResetPasswordAsync(firstName, lastName, email, false);
                else throw new Exception("Second try to add new user - failed.", ex);
            }

            _logger.LogInformation("Add User and Reset Password: Done.");
            return newUserId;
        }

        public async Task UpdateUserAccessAsync(string userId, bool isEnabled)
        {
            _logger.LogInformation("Update User Access: Getting token...");
            await EnrichWithTokenAsync();

            _logger.LogInformation("Update User Access: Updating...");

            var bodyParams = new Dictionary<string, object>();
            bodyParams.Add("accountEnabled", isEnabled);
            var bodyJson = JsonConvert.SerializeObject(bodyParams);
            var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PatchAsync($"{BaseUrl}/users/{userId}", data);
            await ProcessResponse(response, contentAsString =>
            {
                return "success";
            });

            _logger.LogInformation("Update User Access: Done.");
        }

        public async Task DeleteUserAsync(string azureId)
        {
            _logger.LogInformation("Delete User Access: Getting token...");
            await EnrichWithTokenAsync();

            _logger.LogInformation("Delete User Access: Deleting...");

            HttpResponseMessage response = await _httpClient.DeleteAsync($"{BaseUrl}/users/{azureId}");
            await ProcessResponse(response, contentAsString =>
            {
                return "success";
            });

            _logger.LogInformation("Delete User: Done.");
        }

        private async Task<string> AddUserAsync(string firstName, string lastName, string email)
        {
            Dictionary<string, object> bodyParams = CreateRequestBodyForAddUser(firstName, lastName, email);
            var bodyJson = JsonConvert.SerializeObject(bodyParams);
            var data = new StringContent(bodyJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _httpClient.PostAsync($"{BaseUrl}/users", data);
            return await ProcessResponse(response, contentAsString =>
            {
                dynamic result = JsonConvert.DeserializeObject(contentAsString)!;
                return (string)result.id;
            });
        }

        private Dictionary<string, object> CreateRequestBodyForAddUser(string firstName, string lastName, string email)
        {
            var result = new Dictionary<string, object>();

            var identitiesParams = new Dictionary<string, object>();
            identitiesParams.Add("signInType", "emailAddress");
            identitiesParams.Add("issuer", _graphAPIConfig.Issuer);
            identitiesParams.Add("issuerAssignedId", email);
            result.Add("identities", new[] { identitiesParams });

            result.Add("accountEnabled", true);
            result.Add("displayName", $"{firstName} {lastName}");
            result.Add("givenName", firstName);
            result.Add("jobTitle", "Product Marketing Manager"); // TODO: do we need it at all?
            result.Add("mail", email);
            result.Add("mailNickname", "primary");
            result.Add("surname", lastName);

            var passwordProfileParams = new Dictionary<string, object>();
            passwordProfileParams.Add("forceChangePasswordNextSignIn", true);
            passwordProfileParams.Add("password", Guid.NewGuid().ToString()); // TODO: what should be here?
            result.Add("passwordProfile", passwordProfileParams);

            return result;
        }

        private async Task EnrichWithTokenAsync()
        {
            var accessToken = await _cache.GetRecordAsync<string>(CacheKeys.GraphToken);
            if (accessToken is null)
            {
                accessToken = await AcquireTokenAsync();
                await _cache.SetRecordAsync(CacheKeys.GraphToken, accessToken, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Medium)
                });
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private async Task<string> AcquireTokenAsync()
        {
            string authority = $"https://login.microsoftonline.com/{_graphAPIConfig.TenantId}";
            Dictionary<string, string> bodyParams = CreateRequestBodyForAccessToken(_graphAPIConfig.AppClientId, _graphAPIConfig.AppClientSecret);
            var data = new FormUrlEncodedContent(bodyParams);
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                HttpResponseMessage response = await httpClient.PostAsync($"{authority}/oauth2/v2.0/token", data);
                return await ProcessResponse(response, contentAsString =>
                {
                    dynamic result = JsonConvert.DeserializeObject(contentAsString)!;
                    return result.access_token;
                });
            }
        }

        private Dictionary<string, string> CreateRequestBodyForAccessToken(Guid clientId, string clientSecret)
        {
            var result = new Dictionary<string, string>();
            result.Add("grant_type", "client_credentials");
            result.Add("scope", "https://graph.microsoft.com/.default");
            result.Add("client_id", clientId.ToString());
            result.Add("client_secret", clientSecret);
            return result;
        }

        private async Task<T> ProcessResponse<T>(HttpResponseMessage response, Func<string, T> onSuccess, Func<T> onFail = null)
        {
            string contentAsString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return onSuccess(contentAsString);
            }
            else
            {
                if (onFail == null)
                {
                    throw new Exception(contentAsString);
                }
                else
                {
                    return onFail();
                }
            }
        }
    }
}