using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.EmailAlert;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class EmailAlertApiService : IEmailAlertApiService
    {
        private readonly ILogger<EmailAlertApiService> _logger;
        private readonly IMapper _mapper;
        private readonly IEmailAlertService _emailAlertService;

        public EmailAlertApiService(ILogger<EmailAlertApiService> logger,
            IMapper mapper,
            IEmailAlertService emailAlertService)
        {
            _logger = logger;
            _mapper = mapper;
            _emailAlertService = emailAlertService;
        }

        public async Task<WrapperModel<EmailAlertResponse>> GetEmailAlertsAsync()
        {
            WrapperModel<EmailAlertDTO> alerts = await _emailAlertService.GetEmailAlertsAsync();

            return new WrapperModel<EmailAlertResponse>
            {
                Count = alerts.Count,
                DataList = alerts.DataList.Select(_mapper.Map<EmailAlertResponse>)
            };
        }

        public async Task UpdateEmailAlertsAsync(EmailAlertRequest model, int? userId = null)
        {
            IEnumerable<EmailAlertDTO> dataToUpdate = model.EmailAlertsData.Select(_mapper.Map<EmailAlertDTO>);
            if (userId.HasValue)
                await _emailAlertService.UpdateUserEmailAlertsAsync(userId.Value, dataToUpdate);
            else await _emailAlertService.UpdateEmailAlertsAsync(dataToUpdate);
        }

        public async Task PatchEmailAlertAsync(int id, JsonPatchDocument patchDoc)
        {
            await _emailAlertService.PatchEmailAlertAsync(id, patchDoc);
        }

        public async Task<WrapperModel<EmailAlertResponse>> GetUserEmailAlertsAsync(int userId)
        {
            WrapperModel<EmailAlertDTO> alerts = await _emailAlertService.GetUserEmailAlertsAsync(userId);

            return new WrapperModel<EmailAlertResponse>
            {
                Count = alerts.Count,
                DataList = alerts.DataList.Select(_mapper.Map<EmailAlertResponse>)
            };
        }

        public async Task PatchUserEmailAlertAsync(int userId, int id, JsonPatchDocument patchDoc)
        {
            await _emailAlertService.PatchUserEmailAlertAsync(userId, id, patchDoc);
        }
    }
}
