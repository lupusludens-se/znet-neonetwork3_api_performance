using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class UserProfileServiceBlobDecorator : AbstractUserProfileServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public UserProfileServiceBlobDecorator(
            IUserProfileService userProfileService,
            IBlobServicesFacade blobServicesFacade)
            : base(userProfileService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<UserProfileDTO?> GetUserProfileAsync(int id, int userId, string? expand)
        {
            UserProfileDTO? userProfileDTO = await base.GetUserProfileAsync(id, userId, expand);

            await _blobServicesFacade.PopulateWithBlobAsync(userProfileDTO, dto => dto?.User?.Image, (dto, b) => { if (dto?.User != null) dto.User.Image = b; });

            var followersDTOs = userProfileDTO.Followers.ToList();
            foreach (var item in followersDTOs)
            {
                await _blobServicesFacade.PopulateWithBlobAsync(item, dto => dto.Image, (dto, b) => { if (dto?.Company != null) dto.Image = b; });
            } 

            return userProfileDTO;
        }
    }
}
