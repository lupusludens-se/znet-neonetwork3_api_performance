using AutoMapper;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.Tool;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.EmailTemplates.Models;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;
namespace SE.Neo.Infrastructure.Decorators
{
    public class ToolServiceEmailDecorator : AbstractToolServiceDecorator
    {
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly SolarQuoteEmailConfig _config;

        public ToolServiceEmailDecorator(
            IToolService toolService,
            IUserService userService,
            IEmailService emailService,
            IMapper mapper,
            IOptions<SolarQuoteEmailConfig> config) : base(toolService)
        {
            _emailService = emailService;
            _mapper = mapper;
            _userService = userService;
            _config = config.Value;
        }

        public override async Task<int> CreateSolarQuoteAsync(SolarQuoteDTO modelDTO)
        {
            int id = await base.CreateSolarQuoteAsync(modelDTO);
            if (id > 0)
            {
                var emailTemplateModel = _mapper.Map<SolarQuoteEmailTemplatedModel>(modelDTO);
                UserDTO userDTO = await _userService.GetUserAsync(modelDTO.RequestedByUserId, "company");
                if (userDTO == null)
                    throw new CustomException(CoreErrorMessages.EntityNotFound);
                emailTemplateModel.RequestedUserName = $"{userDTO.LastName}, {userDTO.FirstName}";
                emailTemplateModel.CompanyName = userDTO.Company.Name;
                emailTemplateModel.UserEmail = userDTO.Email;

                string subject = string.Format(_config.Subject, emailTemplateModel.CompanyName);
                await _emailService.SendTemplatedEmailAsync(_config.To, subject, emailTemplateModel);
                return id;
            }
            else
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving);
            }
        }
    }
}
