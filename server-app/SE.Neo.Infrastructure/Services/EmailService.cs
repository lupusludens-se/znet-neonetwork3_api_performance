using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Utils;
using Newtonsoft.Json;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Media;
using SE.Neo.Core.Configs;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Enums;
using SE.Neo.EmailTemplates.Models;
using SE.Neo.EmailTemplates.Models.BaseModel;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Constants;
using SE.Neo.Infrastructure.Services.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Web;
using ActionContext = Microsoft.AspNetCore.Mvc.ActionContext;

namespace SE.Neo.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig _config;
        private readonly BaseAppConfig _baseAppConfig;
        private readonly ILogger<EmailService> _logger;
        private readonly IRenderEmailTemplateService _renderEmailTemplateService;
        private readonly IAzureStorageBlobService _azureStorageBlobService;
        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly EmailAssetsConfig _emailAssetsConfig;
        private readonly AzureStorageConfig _azureStorageConfig;
        private readonly IDistributedCache _cache;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;
        private readonly IUnsubscribeEmailService _unSubscribeEmailService;
        private readonly UnsubscribeSettingsConfig _unsubscribeSettingsConfig;

        public EmailService(
            IOptions<EmailConfig> config,
            IOptions<BaseAppConfig> baseAppConfig,
            ILogger<EmailService> logger,
            IRenderEmailTemplateService renderEmailTemplateService,
            IAzureStorageBlobService azureStorageBlobService,
            IActionContextAccessor actionContextAccessor,
            IOptions<EmailAssetsConfig> emailAssetsConfigOptions,
            IOptions<AzureStorageConfig> azureStorageConfigOptions,
            IDistributedCache cache,
            IUnsubscribeEmailService unSubscribeEmailService,
            IOptions<UnsubscribeSettingsConfig> unsubscribeSettingsConfig,
            IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp)
        {
            _config = config.Value;
            _baseAppConfig = baseAppConfig.Value;
            _logger = logger;
            _renderEmailTemplateService = renderEmailTemplateService;
            _azureStorageBlobService = azureStorageBlobService;
            _actionContextAccessor = actionContextAccessor;
            _emailAssetsConfig = emailAssetsConfigOptions.Value;
            _azureStorageConfig = azureStorageConfigOptions.Value;
            _cache = cache;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
            _unSubscribeEmailService = unSubscribeEmailService;
            _unsubscribeSettingsConfig = unsubscribeSettingsConfig.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string htmlMessage)
        {
            await ExecuteAsync(to, subject, htmlMessage);
        }

        public async Task SendTemplatedEmailAsync(string to, string subject, BaseTemplatedEmailModel model, ActionContext? context, bool includeCC = false, UnsubscribeEmailType unsubscribeType = UnsubscribeEmailType.NA, int userId = 0)
        {
            try
            {
                _logger.LogInformation($"Sending {subject} to {to}");
                if (context == null)
                {
                    context = _actionContextAccessor.ActionContext ?? throw new Exception(InfraErrorMessages.ActionContextIsMissed);
                }
                if (model is NotificationTemplateEmailModel template)
                {
                    SetFontLinks(template);
                }
                _logger.LogInformation($"Summary Template for the user name with subject {subject} and to {to}  {JsonConvert.SerializeObject(model)}.");

                string body = await _renderEmailTemplateService.RenderEmailTemplateAsync(model, context);

                if (unsubscribeType == UnsubscribeEmailType.SummaryEmail)
                {
                    string unsubscribeParam = await Task.Run(() => _unSubscribeEmailService.EncryptAsync(Convert.ToString(userId)));
                    string unsubscribeLink = string.Format(_baseAppConfig.BaseAppUrlPattern, _unsubscribeSettingsConfig.UnsubscribeEmailUrlPattern, HttpUtility.UrlEncode(unsubscribeParam));
                    body = body.Replace("_unsubscribeLink_", unsubscribeLink);
                }

                await ExecuteAsync(to, subject, HttpUtility.HtmlDecode(body), includeCC);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while generating the template data for the {subject} to {to}. Error: {ex.Message}. Detailed Message: {ex.InnerException?.Message}");
                throw new Exception($"Failed attempting to send {subject} to {to}.", ex);
            }
        }

        private async Task ExecuteAsync(string to, string subject, string htmlMessage, bool? includeCC = false)
        {
            var imagesContent = await GenerateMailImagesContent();

            var builder = new BodyBuilder();
            List<Attachment> attachments = new();

            foreach (var imagePath in _emailAssetsConfig.Logos)
            {
                var exists = htmlMessage.IndexOf(imagePath) > -1;
                if (exists && imagesContent.ContainsKey(imagePath))
                {
                    MimeEntity image = builder.LinkedResources.Add(imagePath, imagesContent[imagePath]);
                    image.ContentId = MimeUtils.GenerateMessageId();
                    htmlMessage = htmlMessage.Replace(imagePath, $"cid:{image.ContentId}");

                    var extension = imagePath.Split('.').Last();


                    attachments.Add(new Attachment()
                    {
                        Disposition = "inline",
                        ContentId = image.ContentId,
                        Filename = imagePath,
                        Type = $"image/{extension}",
                        Content = Convert.ToBase64String(imagesContent[imagePath])
                    });
                }
            }
            builder.HtmlBody = htmlMessage;

            await SendMailAsync(to, subject, htmlMessage, attachments, includeCC);
        }

        private async Task SendMailAsync(string to, string subject, string htmlMessage, IEnumerable<Attachment> attachments, bool? includeCC = false)
        {
            try
            {
                SendGridClient sendGridClient = new(_config.SendgridAPIKey);
                var fromAddress = new EmailAddress(_config.SenderEmail, _config.SenderName);
                var toAddress = new EmailAddress(to);

                var mailMsg = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, string.Empty, htmlMessage);
                mailMsg.AddCategory(_config.SenderName);

                var replyToAddress = new EmailAddress(_config.ReplyTo);
                mailMsg.SetReplyTo(replyToAddress);

                if (includeCC == true)
                {
                    if (!string.IsNullOrEmpty(_config.CCAddressForUnDeliveredEmail))
                    {
                        List<EmailAddress> ccAddress = new();
                        string[] ccSplit = _config.CCAddressForUnDeliveredEmail.Split(";");
                        foreach (var item in ccSplit)
                        {
                            ccAddress.Add(new EmailAddress(item));
                        }
                        mailMsg.AddCcs(ccAddress);
                    }
                }

                _logger.LogInformation($"Mail started sending for the user with subject {subject} and to : {to}");

                if (attachments?.Count() > 0)
                {
                    mailMsg.AddAttachments(attachments);
                }

                var response = await sendGridClient.SendEmailAsync(mailMsg);

                _logger.LogInformation($"Mail sending status : {response.StatusCode} for the user with subject {subject} and to: {to}");

                if (response.IsSuccessStatusCode && (response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                    response.StatusCode == System.Net.HttpStatusCode.OK))
                {
                    return;
                }
                var keyValuePairs = await response.DeserializeResponseBodyAsync();
                _logger.LogInformation($"Mail sending status : {response.StatusCode} and error is: {JsonConvert.SerializeObject(keyValuePairs.Values)} for the user with subject {subject} and to {to}");
                throw new Exception(JsonConvert.SerializeObject(keyValuePairs.Values));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occured during sending an email to {to} with a subject {subject} and body {htmlMessage}. More details are here: {ex.StackTrace}");
                _logger.LogError($"More details are here: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<Dictionary<string, byte[]>> GenerateMailImagesContent()
        {
            var imagesContent = await _cache.GetRecordAsync<Dictionary<string, byte[]>>(CacheKeys.ImagesContent);

            try
            {
                if (imagesContent is null)

                {
                    imagesContent = new Dictionary<string, byte[]>();
                    foreach (var imagePath in _emailAssetsConfig.Logos)
                    {
                        BlobBaseDTO blobBaseDTO = new BlobBaseDTO() { Name = imagePath, ContainerName = _emailAssetsConfig.AssetsBlobContainer };

                        using (var blobStream = await _azureStorageBlobService.GetBlobStream(blobBaseDTO))
                        {
                            if (blobStream == null)
                            {

                                _logger.LogInformation($"Not able to read a blob {blobBaseDTO.Name} in the {blobBaseDTO.ContainerName} container.");
                                throw new CustomException(CoreErrorMessages.ErrorOnReading);
                            }

                            imagesContent.Add(imagePath, ConvertStreamToByteArray(blobStream));
                        }
                    }

                    await _cache.SetRecordAsync(CacheKeys.ImagesContent, imagesContent, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1440)
                    });
                    _logger.LogInformation("Blob Image DistributedCache created : " + DateTime.UtcNow);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while generating mail images content");

                throw;
            }

            return imagesContent;
        }

        private static byte[] ConvertStreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private void SetFontLinks(NotificationTemplateEmailModel template)
        {
            template.BoldFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.BoldFont}";
            template.RegularFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.RegularFont}";
            template.LightFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.LightFont}";
        }
    }
}
