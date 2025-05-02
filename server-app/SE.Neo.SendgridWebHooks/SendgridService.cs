using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SE.Neo.WebHooks.Configs;
using SendGrid;
using Response = SendGrid.Response;

namespace SE.Neo.WebHooks
{
    public class SendgridService
    {
        private readonly SettingsConfig _settingsConfig;
        private readonly ILogger _logger;

        public SendgridService(IOptions<SettingsConfig> settingsConfig, ILogger<SendgridService> logger)
        {
            _settingsConfig = settingsConfig.Value;
            _logger = logger;
        }

        public async Task StartSyncAsync()
        {
            _logger.LogInformation("Configuring the sendgrid webhook.");
            try
            {
                Console.WriteLine("Please type the environment, dev/ tst/ preprod/ prod.");

                string environment = Console.ReadLine()?.ToLower();

                Console.WriteLine("Please type the action, (Read, ReadAll, Create, Update, Delete)");

                string actionType = Console.ReadLine()?.ToLower();
                if (environment == "dev" || environment == "tst" || environment == "local")
                {
                    Response response;
                    Console.WriteLine("Are you sure you want to continue (Y/N)?");

                    string confirmation = Console.ReadLine()?.ToLower();

                    if (confirmation == "y")
                    {
                        switch (actionType)
                        {
                            case "read":
                                response = await GetById();
                                break;
                            case "readall":
                                response = await GetAll();
                                break;
                            case "create":
                                response = await Create();
                                break;
                            case "update":
                                response = await Update();
                                break;
                            case "delete":
                                response = await Delete();
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Request is invalid");
                    }
                }
                else
                {
                    Console.WriteLine($"Please contact administrators, to run the script for the env {environment}?");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception Configuring the sendgrid webhook. Error: {ex.Message}");
            }
        }

        private async Task<Response> TestNotification()
        {
            var client = new SendGridClient(_settingsConfig.SendgridAPIKey);
            var settings = new
            {
                id = _settingsConfig.EventWebhookId,
                url = "https://localhost:7203/zeigonetwork/local/api/webhook/sendgrid-response"
            };

            string jsonContent = JsonConvert.SerializeObject(settings);

            var response = await client.RequestAsync(
                method: SendGridClient.Method.POST,
                urlPath: "user/webhooks/event/test",
                requestBody: jsonContent
            );
            return response;
        }

        private async Task<Response> Create()
        {
            var client = new SendGridClient(_settingsConfig.SendgridAPIKey);
            var settings = new
            {
                enabled = true,
                spam_report = true,
                bounce = true,
                deferred = true,
                processed = true,
                dropped = true,
                url = $"{_settingsConfig.ApiUrl}{_settingsConfig.SendgridWebhook}"
            };

            string jsonContent = JsonConvert.SerializeObject(settings);

            var response = await client.RequestAsync(
                method: SendGridClient.Method.POST,
                urlPath: "user/webhooks/event/settings",
                requestBody: jsonContent
            );
            return response;
        }

        private async Task<Response> Delete()
        {
            var client = new SendGridClient(_settingsConfig.SendgridAPIKey);
            return await client.RequestAsync(
                method: SendGridClient.Method.DELETE,
                urlPath: $"user/webhooks/event/settings/{_settingsConfig.EventWebhookId}"
            );
        }

        private async Task<Response> Update()
        {
            var client = new SendGridClient(_settingsConfig.SendgridAPIKey);
            var settings = new
            {
                enabled = true,
                spam_report = true,
                bounce = true,
                deferred = true,
                processed = true,
                dropped = true,
                url = $"{_settingsConfig.ApiUrl}{_settingsConfig.SendgridWebhook}"
            };

            string jsonContent = JsonConvert.SerializeObject(settings);


            var response = await client.RequestAsync(
                method: SendGridClient.Method.PATCH,
                urlPath: $"user/webhooks/event/settings/{_settingsConfig.EventWebhookId}",
                requestBody: jsonContent
            );
            return response;
        }

        private async Task<Response> GetById()
        {
            var client = new SendGridClient(_settingsConfig.SendgridAPIKey);
            var response = await client.RequestAsync(
                method: SendGridClient.Method.GET,
                urlPath: $"user/webhooks/event/settings/{_settingsConfig.EventWebhookId}"
            );
            return response;
        }

        private async Task<Response> GetAll()
        {
            var client = new SendGridClient(_settingsConfig.SendgridAPIKey);

            return await client.RequestAsync(
                method: SendGridClient.Method.GET,
                urlPath: "user/webhooks/event/settings/all"
            );
        }

    }
}