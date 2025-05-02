using Microsoft.AspNetCore.Http;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services
{
    public class CurrentUserResolverService : ICurrentUserResolverService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserResolverService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public BaseUserModel? GetCurrentUser()
        {
            return (BaseUserModel?)_httpContextAccessor.HttpContext?.Items["User"];
        }
    }
}
