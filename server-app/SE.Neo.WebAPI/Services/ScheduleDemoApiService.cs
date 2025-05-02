using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SE.Neo.EmailTemplates.Models;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Models.ScheduleDemo;
using IScheduleDemoApiService = SE.Neo.WebAPI.Services.Interfaces.IScheduleDemoApiService;

namespace SE.Neo.WebAPI.Services
{
    public class ScheduleDemoApiService : IScheduleDemoApiService
    {
        private readonly ILogger<ScheduleDemoApiService> _logger;
        private readonly EmailAssetsConfig _emailAssetsConfig;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ScheduleDemoEmailConfig _scheduleDemoEmailConfig;
        private readonly BaseAppConfig _config;

        public ScheduleDemoApiService(ILogger<ScheduleDemoApiService> logger, IMapper mapper, IOptions<ScheduleDemoEmailConfig> scheduleDemoEmailConfigOptions,
            IOptions<EmailAssetsConfig> emailAssetsConfig, IOptions<BaseAppConfig> config,
            IEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _emailAssetsConfig = emailAssetsConfig.Value;
            _emailService = emailService;
            _scheduleDemoEmailConfig = scheduleDemoEmailConfigOptions.Value;
            _config = config.Value;
        }

        public async Task SendScheduleDemoMessageAsync(ScheduleDemoRequest model, ActionContext context)
        {
            var mappedModelForUser = _mapper.Map<ScheduleDemoToUserTemplateModel>(model);
            mappedModelForUser.LogoUrl = _emailAssetsConfig.ZeigoLogo;
            mappedModelForUser.DashboardLink = string.Format($"{_config.BaseAppUrlPattern}", "dashboard", "");
            await _emailService.SendTemplatedEmailAsync(mappedModelForUser.Email, _scheduleDemoEmailConfig.SubjectForUser, mappedModelForUser, context, false);

            var mappedModelForAdmin = _mapper.Map<ScheduleDemoToAdminTemplateModel>(model);
            mappedModelForAdmin.LogoUrl = _emailAssetsConfig.ZeigoLogo;
            await _emailService.SendTemplatedEmailAsync(_scheduleDemoEmailConfig.To, _scheduleDemoEmailConfig.SubjectForAdmin, mappedModelForAdmin, context, false);
        }
    }
}