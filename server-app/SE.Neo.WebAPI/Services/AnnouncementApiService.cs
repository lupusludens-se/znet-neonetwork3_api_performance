using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common;
using SE.Neo.Common.Models.Announcement;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Models.Announcement;
using SE.Neo.WebAPI.Services.Interfaces;

namespace SE.Neo.WebAPI.Services
{
    public class AnnouncementApiService : IAnnouncementApiService
    {
        private readonly IMapper _mapper;
        private readonly IAnnouncementService _announcementService;

        public AnnouncementApiService(
            IMapper mapper,
            IAnnouncementService announcementService)
        {
            _mapper = mapper;
            _announcementService = announcementService;
        }

        public async Task<int> CreateUpdateAnnouncementAsync(AnnouncementRequest model, int id = 0, bool forceActivate = false)
        {
            AnnouncementDTO modelDTO = _mapper.Map<AnnouncementDTO>(model);
            modelDTO.Id = id;
            int announcementId = await _announcementService.CreateUpdateAnnouncementAsync(modelDTO, forceActivate);
            return announcementId;
        }

        public async Task<int> PatchAnnouncementAsync(int id, JsonPatchDocument patchDoc, bool forceActivate = false)
        {
            int announcementId = await _announcementService.PatchAnnouncementAsync(id, patchDoc, forceActivate);
            return announcementId;
        }

        public async Task<AnnouncementResponse?> GetLatestAnnouncementAsync(int audienceId, string? expand)
        {
            AnnouncementDTO? modelDTO = await _announcementService.GetLatestAnnouncementAsync(audienceId, expand);
            if (audienceId == -1)
            {
                if (modelDTO == null)
                {
                    modelDTO = new AnnouncementDTO
                    {
                        Name = ZnConstants.DefaultAnnouncementText,
                        ButtonText = ZnConstants.DefaultAnnouncementButtonText
                    };
                }
                else
                {
                    return _mapper.Map<AnnouncementResponse>(modelDTO, opt =>
                    {
                        opt.AfterMap((src, dest) => dest.ButtonUrl = "");
                    });
                }
            }
            return _mapper.Map<AnnouncementResponse>(modelDTO);
        }

        public async Task<AnnouncementResponse?> GetAnnouncementAsync(int id, string? expand)
        {
            AnnouncementDTO? modelDTO = await _announcementService.GetAnnouncementAsync(id, expand);
            return _mapper.Map<AnnouncementResponse>(modelDTO);
        }

        public async Task<WrapperModel<AnnouncementResponse>> GetAnnouncementsAsync(ExpandOrderModel expandOrderModel)
        {
            WrapperModel<AnnouncementDTO> announcementsResult = await _announcementService.GetAnnouncementsAsync(expandOrderModel);

            var wrapper = new WrapperModel<AnnouncementResponse>
            {
                Count = announcementsResult.Count,
                DataList = announcementsResult.DataList.Select(_mapper.Map<AnnouncementResponse>)
            };
            return wrapper;
        }

        public async Task RemoveAnnouncementAsync(int id)
        {
            await _announcementService.RemoveAnnouncementAsync(id);
        }
    }
}
