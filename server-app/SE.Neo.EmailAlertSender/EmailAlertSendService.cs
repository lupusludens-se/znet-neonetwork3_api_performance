using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Service.Interfaces;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.EmailAlertSender.Configs;
using SE.Neo.EmailAlertSender.Interfaces;
using SE.Neo.EmailAlertSender.Models;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.EmailAlertSender
{
    public partial class EmailAlertSendService : TimeZoneHelper, IEmailAlertSendService
    {
        private readonly EmailConfig _config;
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ISendgridService _sendgridService;
        private readonly BaseAppConfig _baseAppConfig;
        private readonly EmailAlertConfig _emailAlertConfig;
        private readonly ILogger<EmailAlertSendService> _logger;
        private readonly EmailAssetsConfig _emailAssetsConfig;
        private readonly IAzureStorageBlobService _azureStorageBlobService;
        private readonly SummaryEmailSettingsConfig _summaryEmailSettingsConfig;
        public EmailAlertSendService(ApplicationContext context, IMapper mapper,
            IEmailService emailService, IOptions<EmailAlertConfig> emailAlertConfigOptions, IOptions<BaseAppConfig> baseAppConfigOptions,
            ILogger<EmailAlertSendService> logger, IOptions<EmailAssetsConfig> emailAssetsConfig, IAzureStorageBlobService azureStorageBlobService, IOptions<SummaryEmailSettingsConfig> summaryEmailSettingsConfig,
            ISendgridService sendgridService, IOptions<EmailConfig> config)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _emailAlertConfig = emailAlertConfigOptions.Value;
            _baseAppConfig = baseAppConfigOptions.Value;
            _logger = logger;
            _emailAssetsConfig = emailAssetsConfig.Value;
            _azureStorageBlobService = azureStorageBlobService;
            _sendgridService = sendgridService;
            _config = config.Value;
            _summaryEmailSettingsConfig = summaryEmailSettingsConfig.Value;
        }

        public async Task SendEmailsAsync(ActionContext actionContext)
        {
            _logger.LogInformation("Email alert service starting.");

            DateTime now = DateTime.UtcNow;
            JobLog? lastRunLog = null;

            try
            {
                lastRunLog = await _context.JobLogs
                    .OrderBy(jl => jl.LastRunTime)
                    .LastOrDefaultAsync();
                if (lastRunLog == null)
                {
                    _logger.LogInformation("No prior job log found. Creating and exiting.");
                    await _context.JobLogs.AddAsync(new JobLog { LastRunTime = DateTime.MinValue, Success = true, JobType = JobType.EmailAlertSender });
                    await _context.SaveChangesAsync();
                    return;
                }

                DateTime lastRun = lastRunLog.LastRunTime;

                // skip daily users if job has already run today
                bool daily = SendDaily(lastRun, now);
                // skip weekly if job has run since first sunday of this week
                bool weekly = SendWeekly(lastRun, now);
                // skip monthly if job has run since 1st of the month
                bool monthly = SendMonthly(lastRun, now);

                if (daily)
                {
                    await SendAlertForUndeliveredMailsAsync(actionContext);
                }

                //_logger.LogInformation($"Email alert: last run time = {lastRun}, daily = {daily}, weekly = {weekly}, monthly = {monthly}");

                // creating the image distributed cache in case it is not present
                await _emailService.GenerateMailImagesContent();

                // get all user email alert settings
                List<UserEmailAlertItem> alertsToSend = await _context.UserEmailAlerts
                    .AsNoTracking()
                    .Include(uea => uea.EmailAlert)
                    .Include(uea => uea.User)
                    .ThenInclude(u => u.UserProfile)
                    .Where(uea => uea.User.StatusId == Core.Enums.UserStatus.Active)
                    .Select(uea => new UserEmailAlertItem
                    {
                        EmailAlertCategory = uea.EmailAlert.Category,
                        EmailAlertFrequency = uea.Frequency,
                        UserEmailAddress = uea.User.Email,
                        UserId = uea.UserId,
                        CompanyId = uea.User.CompanyId,
                        UserRegionIds = uea.User.UserProfile.Regions.Select(upr => upr.RegionId),
                        UserCategoryIds = uea.User.UserProfile.Categories.Select(upc => upc.CategoryId),
                        UserTimeZone = uea.User.TimeZone,
                        UserRoleIds = uea.User.Roles.Select(ur => ur.RoleId),
                        UserFirstName = uea.User.FirstName,
                        UserLastName = uea.User.LastName
                    })
                    .ToListAsync();

                if (alertsToSend.Any())
                {
                    // Log
                    // Updating the last run time to avoid multiple altert triggering
                    lastRunLog.LastRunTime = now;
                    lastRunLog.Success = true;
                    await _context.SaveChangesAsync();
                }

                // send immediate onetime non-summary event alerts
                _logger.LogInformation("Starting one-time event alerts.");
                await SendEventAlertsAsync(lastRun, now, actionContext);

                // send reminders to complete profile
                _logger.LogInformation("Starting profile completion reminders.");
                await SendOnboardingReminders(lastRun, now, actionContext);

                // New Message Alerts
                List<UserEmailAlertItem> messageAlerts = alertsToSend.Where(uea => uea.EmailAlertFrequency != EmailAlertFrequency.Off)
                                                        .Where(alert => alert.EmailAlertCategory == EmailAlertCategory.Messaging).ToList();
                if (messageAlerts.Any())
                {
                    _logger.LogInformation("Starting message alerts.");
                    _logger.LogInformation($"{messageAlerts.Count()} message alerts alerts found");
                    await SendNewMessageAlertsAsync(messageAlerts, lastRun, now, actionContext);
                }

                //  Forum Response Alerts
                List<UserEmailAlertItem> forumAlerts = alertsToSend.Where(uea => uea.EmailAlertFrequency == EmailAlertFrequency.Immediately ||
                                                        (uea.EmailAlertFrequency == EmailAlertFrequency.Daily && daily) ||
                                                        (uea.EmailAlertFrequency == EmailAlertFrequency.Weekly && weekly) ||
                                                        (uea.EmailAlertFrequency == EmailAlertFrequency.Monthly && monthly))
                                                        .Where(alert => alert.EmailAlertCategory == EmailAlertCategory.ForumResponse).ToList();
                if (forumAlerts.Any())
                {
                    _logger.LogInformation("Starting forum response alerts.");
                    _logger.LogInformation($"{forumAlerts.Count()} forum alerts alerts found");
                    await SendForumResponseAlertsAsync(forumAlerts, lastRun, now, actionContext);
                }

                // Summary Alerts
                List<UserEmailAlertItem> summaryAlerts = alertsToSend.Where(alert => alert.EmailAlertCategory == EmailAlertCategory.Summary)
                                      .ToList();
                if (summaryAlerts.Any())
                {
                    _logger.LogInformation("Starting summary alerts.");
                    _logger.LogInformation($"{summaryAlerts.Count()} summary alerts alerts found");
                    await SendSummaryAlertsAsync(summaryAlerts, lastRun, now, actionContext);
                }
                _logger.LogInformation("Email alert service completed successful run.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Email alert job did not run to completion.");
                if (lastRunLog != null)
                {
                    lastRunLog.Success = false;
                    lastRunLog.JobType = JobType.EmailAlertSender;
                    lastRunLog.LastRunTime = now;
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}