using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public partial class EmailAlertService : IEmailAlertService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<EmailAlertService> _logger;
        private readonly IMapper _mapper;

        public EmailAlertService(ApplicationContext context, ILogger<EmailAlertService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<WrapperModel<EmailAlertDTO>> GetEmailAlertsAsync()
        {
            IQueryable<EmailAlert> alertsQuery = _context.EmailAlerts.AsNoTracking();

            List<EmailAlert> alerts = await alertsQuery
                .ToListAsync();

            return new WrapperModel<EmailAlertDTO>
            {
                Count = alerts.Count,
                DataList = alerts.Select(_mapper.Map<EmailAlertDTO>)
            };
        }

        public async Task UpdateEmailAlertsAsync(IEnumerable<EmailAlertDTO> dataToUpdate)
        {
            IEnumerable<EmailAlert> emailAlerts = await _context.EmailAlerts
                .Where(ea => dataToUpdate.Select(d => d.Id).Contains(ea.Id))
                .ToListAsync();

            foreach (EmailAlert alert in emailAlerts)
            {
                EmailAlertDTO? alertDTO = dataToUpdate.SingleOrDefault(d => d.Id == alert.Id);
                if (alertDTO != null)
                {
                    if (alert.Category == EmailAlertCategory.Summary && alertDTO.Frequency == EmailAlertFrequency.Immediately)
                        throw new CustomException("Immediate frequency invalid for summary alerts.");
                    alert.Frequency = alertDTO.Frequency;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task PatchEmailAlertAsync(int id, JsonPatchDocument patchDoc)
        {
            EmailAlert? emailAlert = await _context.EmailAlerts.SingleOrDefaultAsync(ea => ea.Id == id);
            if (emailAlert != null)
            {
                if (emailAlert.Category == EmailAlertCategory.Summary)
                {
                    long newFrequency = (long)patchDoc.Operations.Select(o => o.value).FirstOrDefault()!;
                    if (newFrequency == (int)EmailAlertFrequency.Immediately)
                    {
                        throw new CustomException("Immediately frequency invalid for Summary");
                    }
                }
                try
                {
                    patchDoc.ApplyTo(emailAlert);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }

        public async Task<WrapperModel<EmailAlertDTO>> GetUserEmailAlertsAsync(int userId)
        {
            IQueryable<UserEmailAlert> userEmailAlertsQuery = _context.UserEmailAlerts.AsNoTracking()
                .Include(uea => uea.EmailAlert)
                .Where(uea => uea.UserId == userId);

            List<UserEmailAlert> userEmailAlerts = await userEmailAlertsQuery
                .ToListAsync();

            var emailAlertDTOs = new List<EmailAlertDTO>();

            foreach (var userEmailAlert in userEmailAlerts)
            {
                var emailAlert = _mapper.Map<EmailAlertDTO>(userEmailAlert.EmailAlert);
                emailAlert.Id = userEmailAlert.Id;
                emailAlert.Frequency = userEmailAlert.Frequency;
                emailAlertDTOs.Add(emailAlert);
            }

            return new WrapperModel<EmailAlertDTO>
            {
                Count = emailAlertDTOs.Count,
                DataList = emailAlertDTOs
            };
        }

        public async Task UpdateUserEmailAlertsAsync(int userId, IEnumerable<EmailAlertDTO> dataToUpdate)
        {
            IEnumerable<UserEmailAlert> userEmailAlerts = await _context.UserEmailAlerts
                .Include(uea => uea.EmailAlert)
                .Where(uea => uea.UserId == userId && dataToUpdate.Select(d => d.Id).Contains(uea.Id))
                .ToListAsync();

            foreach (UserEmailAlert alert in userEmailAlerts)
            {
                EmailAlertDTO? alertDTO = dataToUpdate.SingleOrDefault(d => d.Id == alert.Id);
                if (alertDTO != null)
                {
                    if (alert.EmailAlert.Category == EmailAlertCategory.Summary && alertDTO.Frequency == EmailAlertFrequency.Immediately)
                        throw new CustomException("Immediate frequency invalid for summary alerts.");
                    alert.Frequency = alertDTO.Frequency;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task PatchUserEmailAlertAsync(int userId, int id, JsonPatchDocument patchDoc)
        {
            UserEmailAlert? userEmailAlert = await _context.UserEmailAlerts.Include(uea => uea.EmailAlert).SingleOrDefaultAsync(uea => uea.Id == id && uea.UserId == userId);
            if (userEmailAlert != null)
            {
                if (userEmailAlert.EmailAlert.Category == EmailAlertCategory.Summary)
                {
                    long newFrequency = (long)patchDoc.Operations.Select(o => o.value).FirstOrDefault()!;
                    if (newFrequency == (int)EmailAlertFrequency.Immediately)
                    {
                        throw new CustomException("Immediately frequency invalid for Summary");
                    }
                }
                try
                {
                    patchDoc.ApplyTo(userEmailAlert);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }
    }
}
