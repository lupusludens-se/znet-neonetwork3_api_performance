using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using SE.Neo.DataSync.Configs;
using System.Net.Http.Headers;
using System.Security;

namespace SE.Neo.DataSync
{
    public class DataSyncService
    {
        private readonly SettingsConfig _settingsConfig;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public DataSyncService(IOptions<SettingsConfig> settingsConfig, HttpClient httpClient, ILogger<DataSyncService> logger)
        {
            _settingsConfig = settingsConfig.Value;
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task StartSyncAsync(string[] args)
        {
            _logger.LogInformation("Starting data sync.");
            try
            {
                string authority = _settingsConfig.AuthorityUrl;
                string[] scopes = new string[] { _settingsConfig.Scope };

                IPublicClientApplication app = PublicClientApplicationBuilder.Create(_settingsConfig.AppClientId)
                      .WithAuthority(authority)
                      .Build();

                var accounts = await app.GetAccountsAsync();
                AuthenticationResult? result = null;

                if (accounts.Any())
                {
                    result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                      .ExecuteAsync();
                }
                else
                {
                    var securePassword = new SecureString();
                    foreach (char c in _settingsConfig.UserPassword)        // you should fetch the password
                        securePassword.AppendChar(c);                       // keystroke by keystroke

                    result = await app.AcquireTokenByUsernamePassword(scopes,
                                                                     _settingsConfig.UserName,
                                                                      securePassword)
                                       .ExecuteAsync();
                }
                List<Task> _tasks = new List<Task>();
                _tasks.Add(SendHttpDataSyncRequest(result.AccessToken));
                _tasks.Add(SendHttpUserLoginSyncRequest(result.AccessToken));

                await Task.WhenAll(_tasks);
            }
            catch (MsalException ex)
            {
                _logger.LogError(ex, "Exception getting identity token.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Sync Requests Stopped, due to an error");
            }
        }

        private async Task SendHttpDataSyncRequest(string accessToken)
        {
            try
            {
                _logger.LogInformation("Sending http sync request.");
                using (var request = new HttpRequestMessage(HttpMethod.Post, _settingsConfig.ApiUrl + "/articles/sync"))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var response = await _httpClient.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Articles: Sync finish successful.");
                    }
                    else
                    {
                        var result_string = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Sync error:{result_string}");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Data Sync HTTP Request Stopped, due to an error");
            }
        }

        private async Task SendHttpUserLoginSyncRequest(string accessToken)
        {
            try
            {
                using (var userSyncrequest = new HttpRequestMessage(HttpMethod.Post, _settingsConfig.ApiUrl + "/users/synclogincount"))
                {
                    userSyncrequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var response = await _httpClient.SendAsync(userSyncrequest);

                    if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Users: Sync finish successful");
                    }
                    else
                    {
                        var result_string = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Sync error:{result_string}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Users Login Count Sync HTTP Request Stopped, due to an error ");
            }
        }
    }
}