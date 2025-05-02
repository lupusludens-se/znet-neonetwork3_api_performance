using SE.Neo.Common.Models.User;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class UserPendingAzureDecorator : AbstractUserPendingDecorator
    {
        private readonly IGraphAPIService _graphAPIService;

        public UserPendingAzureDecorator(
            IUserPendingService userPendingService,
            IGraphAPIService graphAPIService
           ) : base(userPendingService)
        {
            _graphAPIService = graphAPIService;
        }

        public override async Task<UserDTO> ApproveUserPendingAsync(UserPendingDTO userPendingDTO)
        {
            string azureId = await _graphAPIService.AddUserAndResetPasswordAsync(userPendingDTO.FirstName, userPendingDTO.LastName, userPendingDTO.Email);

            userPendingDTO.AzureId = azureId;

            return await base.ApproveUserPendingAsync(userPendingDTO);
        }
    }
}
