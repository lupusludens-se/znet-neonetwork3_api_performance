using SE.Neo.Common.Models.User;

namespace SE.Neo.Infrastructure.Services.Interfaces
{
    public interface IDotDigitalService
    {
        Task CreateContactAndAddUserToAddressBook(UserDTO modelDTO, string consentIp, string consentUserAgent);
    }
}