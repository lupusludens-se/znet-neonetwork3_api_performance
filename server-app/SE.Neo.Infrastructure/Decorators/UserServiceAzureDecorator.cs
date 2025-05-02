using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class UserServiceAzureDecorator : AbstractUserServiceDecorator
    {
        private readonly IGraphAPIService _graphAPIService;

        public UserServiceAzureDecorator(
            IUserService userService,
            IGraphAPIService graphAPIService
           ) : base(userService)
        {
            _graphAPIService = graphAPIService;
        }

        public override async Task<int> CreateUpdateUserAsync(int id, UserDTO userDTO, IEnumerable<EmailAlertDTO> emailAlertDTOs)
        {
            UserDTO? originalUserDTO = null;

            if (id == 0)
            {
                // create new user on Azure
                string azureId = await _graphAPIService.AddUserAndResetPasswordAsync(userDTO.FirstName, userDTO.LastName, userDTO.Username);
                userDTO.AzureId = azureId;
            }
            else
            {
                originalUserDTO = await base.GetUserAsync(id);

                // TODO: need to update user on Azure? if so, which fields should be updated?
            }

            int userId = await base.CreateUpdateUserAsync(id, userDTO, emailAlertDTOs);

            if (originalUserDTO != null)
            {
                bool wasEnabled = IsEnabled(originalUserDTO);
                bool isEnabled = IsEnabled(userDTO);
                if (wasEnabled != isEnabled)
                {
                    // change user's access level on Azure depends on user's status
                    await _graphAPIService.UpdateUserAccessAsync(originalUserDTO.AzureId, isEnabled);
                }
            }

            return userId;
        }

        private bool IsEnabled(UserDTO userDTO)
        {
            return userDTO.StatusId == (int)UserStatus.Onboard || userDTO.StatusId == (int)UserStatus.Active;
        }

        public override async Task<UserDTO?> PatchUserAsync(int id, JsonPatchDocument patchDoc)
        {
            UserDTO? originalUserDTO = await base.GetUserAsync(id);
            UserDTO? userDTO = await base.PatchUserAsync(id, patchDoc);

            if (originalUserDTO != null && userDTO != null)
            {
                bool wasEnabled = IsEnabled(originalUserDTO);
                bool isEnabled = IsEnabled(userDTO);
                if (wasEnabled != isEnabled)
                {
                    // change user's access level on Azure depends on user's status
                    await _graphAPIService.UpdateUserAccessAsync(userDTO.AzureId, isEnabled);
                }
            }

            return userDTO;
        }

        public override async Task DeleteUserAsync(int id)
        {
            UserDTO? userModel = await base.GetUserAsync(id);

            string? userAzureId = userModel?.AzureId;

            if (!string.IsNullOrEmpty(userAzureId))
                await _graphAPIService.DeleteUserAsync(userAzureId);

            await base.DeleteUserAsync(id);
        }
    }
}