using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Core.Configs;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Services.Interfaces;
using System.Net;

namespace SE.Neo.WebAPI.Handlers
{
    public class NeoAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new AuthorizationMiddlewareResultHandler();

        private readonly IUserApiService _userService;
        private readonly IDistributedCache _cache;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;

        public NeoAuthorizationMiddlewareResultHandler(
            IUserApiService userService,
            IDistributedCache cache,
            IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp)
        {
            _userService = userService;
            _cache = cache;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
        }

        public async Task HandleAsync(
            RequestDelegate requestDelegate,
            HttpContext httpContext,
            AuthorizationPolicy authorizationPolicy,
            PolicyAuthorizationResult policyAuthorizationResult)
        {
            // if the authorization was forbidden and the resource had specific requirements,
            // provide a custom response.
            if (Show404ForForbiddenResult(policyAuthorizationResult))
            {
                // Return a 404 to make it appear as if the resource does not exist.
                httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return;
            }

            var userEmail = httpContext.User.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            if (!string.IsNullOrEmpty(userEmail))
            {
                bool hasNotNEO3ROPCClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == "tfp")?.Value != "B2C_1A_ZNROPC";
                if (hasNotNEO3ROPCClaim)
                {
                    string sessionIdAsString = httpContext.User.Claims.FirstOrDefault(x => x.Type == "nonce")?.Value!;
                    Guid sessionId = Guid.Parse(sessionIdAsString);
                    httpContext.Items["SessionId"] = sessionId;
                }

                var currentUser = await _cache.GetRecordAsync<UserModel>(userEmail)
                    ?? await _userService.GetUserModelByUsernameAsync(userEmail);

                if (currentUser == null || currentUser.Status == Core.Enums.UserStatus.Deleted ||
                    (currentUser.Status != Core.Enums.UserStatus.Onboard && currentUser.Status != Core.Enums.UserStatus.Expired && !currentUser.HasUserProfile && hasNotNEO3ROPCClaim))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
                }

                if (currentUser.Status == Core.Enums.UserStatus.Inactive)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Locked;
                    return;
                }

                if (currentUser.RoleIds.Any(x => x == (int)RoleType.SPAdmin) && !currentUser.RoleIds.Any(x => x == (int)RoleType.SolutionProvider))
                {
                    currentUser.RoleIds.Add((int)RoleType.SolutionProvider);
                }

                if (currentUser.RoleIds.Any(x => x == (int)RoleType.SystemOwner) && !currentUser.RoleIds.Any(x => x == (int)RoleType.Admin))
                {
                    currentUser.RoleIds.Add((int)RoleType.Admin);
                }

                await _cache.SetRecordAsync(userEmail, currentUser, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_memoryCacheTimeStamp.Short)
                });


                httpContext.Items["User"] = currentUser;
            }
            else
            {
                httpContext.Items["User"] = null;
                httpContext.Items["SessionId"] = null;
                await requestDelegate(httpContext);
                return;
            }

            // Fallback to the default implementation.
            await _defaultHandler.HandleAsync(requestDelegate, httpContext, authorizationPolicy, policyAuthorizationResult);
        }

        private bool Show404ForForbiddenResult(PolicyAuthorizationResult policyAuthorizationResult)
        {
            return policyAuthorizationResult.Forbidden && policyAuthorizationResult.AuthorizationFailure.FailedRequirements.OfType<Show404Requirement>().Any();
        }
    }

    public class Show404Requirement : IAuthorizationRequirement
    { }
}