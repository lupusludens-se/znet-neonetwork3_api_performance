using SE.Neo.Common.Models.User;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface ICurrentUserResolverService
    {
        public BaseUserModel? GetCurrentUser();
    }
}
