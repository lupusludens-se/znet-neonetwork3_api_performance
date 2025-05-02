using SE.Neo.Common.Models.Announcement;
using SE.Neo.Common.Models.Media;
using SE.Neo.Core.Decorators;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class AnnouncementServiceBlobDecorator : AbstractAnnouncementServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public AnnouncementServiceBlobDecorator(
            IAnnouncementService announcementService,
            IBlobServicesFacade blobServicesFacade)
            : base(announcementService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        private async Task SetBackgroundImageAsync(AnnouncementDTO? announcementDTO)
        {
            await _blobServicesFacade.PopulateWithBlobAsync(announcementDTO, dto => dto?.BackgroundImage, (dto, b) => { if (dto != null) dto.BackgroundImage = b; });
        }

        public override async Task<int> CreateUpdateAnnouncementAsync(AnnouncementDTO modelDTO, bool forceActivate = false)
        {
            bool isUpdate = modelDTO.Id > 0;
            string? oldAnnouncementBackgroundImageName = isUpdate ? (await base.GetAnnouncementAsync(modelDTO.Id))?.BackgroundImageName : null;

            modelDTO.Id = await base.CreateUpdateAnnouncementAsync(modelDTO, forceActivate);

            if (isUpdate)
                if (!string.IsNullOrEmpty(oldAnnouncementBackgroundImageName) && oldAnnouncementBackgroundImageName != modelDTO.BackgroundImageName)
                    await _blobServicesFacade.DeleteBlobAsync(new BlobBaseDTO() { Name = oldAnnouncementBackgroundImageName, ContainerName = BlobType.Announcement.ToString() });

            return modelDTO.Id;
        }

        public override async Task<AnnouncementDTO?> GetLatestAnnouncementAsync(int audienceId, string? expand = null)
        {
            AnnouncementDTO? announcementDTO = await base.GetLatestAnnouncementAsync(audienceId, expand);

            await SetBackgroundImageAsync(announcementDTO);

            return announcementDTO;
        }

        public override async Task<AnnouncementDTO?> GetAnnouncementAsync(int id, string? expand = null)
        {
            AnnouncementDTO? announcementDTO = await base.GetAnnouncementAsync(id, expand);

            await SetBackgroundImageAsync(announcementDTO);

            return announcementDTO;
        }

        public override async Task RemoveAnnouncementAsync(int id)
        {
            string? oldAnnouncementBackgroundImageName = (await base.GetAnnouncementAsync(id))?.BackgroundImageName;

            await base.RemoveAnnouncementAsync(id);

            if (!string.IsNullOrEmpty(oldAnnouncementBackgroundImageName))
                await _blobServicesFacade.DeleteBlobAsync(new BlobBaseDTO() { Name = oldAnnouncementBackgroundImageName, ContainerName = BlobType.Announcement.ToString() });
        }
    }
}
