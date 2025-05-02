using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class UserServiceBlobDecorator : AbstractUserServiceDecorator
    {
        private readonly IBlobServicesFacade _blobServicesFacade;

        public UserServiceBlobDecorator(
            IUserService userService,
            IBlobServicesFacade blobServicesFacade)
            : base(userService)
        {
            _blobServicesFacade = blobServicesFacade;
        }

        public override async Task<int> CreateUpdateUserAsync(int id, UserDTO modelDTO, IEnumerable<EmailAlertDTO> emailAlertDTOs)
        {
            bool isUpdate = id > 0;
            string? oldUserImageName = isUpdate ? (await base.GetUserAsync(id))?.ImageName : null;

            id = await base.CreateUpdateUserAsync(id, modelDTO, emailAlertDTOs);

            if (isUpdate)
                if (!string.IsNullOrEmpty(oldUserImageName) && oldUserImageName != modelDTO.ImageName)
                    await _blobServicesFacade.DeleteBlobAsync(new BlobBaseDTO() { Name = oldUserImageName, ContainerName = BlobType.Users.ToString() });

            return id;
        }

        public override async Task<UserDTO?> GetUserAsync(int id, string? expand, bool accessPrivateInfo = false)
        {
            UserDTO? userDTO = await base.GetUserAsync(id, expand, accessPrivateInfo);

            await _blobServicesFacade.PopulateWithBlobAsync(userDTO, dto => dto?.Image, (dto, b) => { if (dto != null) dto.Image = b; });

            return userDTO;
        }

        public override async Task<WrapperModel<UserDTO>> GetUsersAsync(BaseSearchFilterModel filter, int userId, IEnumerable<int> userRoleIds, int companyId, bool isOwnCompanyUsersRequest = false, bool accessPrivateInfo = false)
        {
            WrapperModel<UserDTO> usersResult = await base.GetUsersAsync(filter, userId, userRoleIds, companyId, isOwnCompanyUsersRequest, accessPrivateInfo);

            List<UserDTO> userDTOs = usersResult.DataList.ToList();
            await _blobServicesFacade.PopulateWithBlobAsync(userDTOs, dto => dto.Image, (dto, b) => dto.Image = b);
            usersResult.DataList = userDTOs;

            return usersResult;
        }

        public override async Task DeleteUserAsync(int id)
        {
            UserDTO? userModel = await base.GetUserAsync(id);

            string? userImageName = userModel?.ImageName;

            await base.DeleteUserAsync(id);

            if (!string.IsNullOrEmpty(userImageName))
                await _blobServicesFacade.DeleteBlobAsync(new BlobBaseDTO() { Name = userImageName, ContainerName = BlobType.Users.ToString() });
        }
    }
}