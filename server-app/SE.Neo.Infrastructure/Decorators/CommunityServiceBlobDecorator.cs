using SE.Neo.Common.Models.Community;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class CommunityServiceBlobDecorator : AbstractCommunityServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;
        public CommunityServiceBlobDecorator(
            IBlobServicesFacade blobServicesFacade,
            ICommunityService communityService) : base(communityService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<WrapperModel<CommunityItemDTO>> GetCommunityAsync(int userId, CommunityFilter filter)
        {
            WrapperModel<CommunityItemDTO> communityResult = await base.GetCommunityAsync(userId, filter);
            List<CommunityItemDTO> communityDTOs = communityResult.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(communityDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
            communityResult.DataList = communityDTOs;
            return communityResult;
        }

    }
}
