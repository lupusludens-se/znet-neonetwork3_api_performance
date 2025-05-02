using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.User;
using System.Net;

namespace SE.Neo.WebAPI.Attributes
{
    /// <summary>
    /// Specifies that the class or method that this attribute is applied to requires authorization based on user roles.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RolesAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly RoleType[] _roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="RolesAttribute"/> class with the specified roles.
        /// </summary>
        /// <param name="roles">The roles that are allowed to access the resource.</param>
        public RolesAttribute(params RoleType[] roles)
        {
            _roles = roles;
        }

        /// <summary>
        /// Called to check if the user is authorized to access the resource.
        /// </summary>
        /// <param name="context">The authorization filter context.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUser = (UserModel?)context.HttpContext.Items["User"];
            if (currentUser == null)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                return;
            }
            if (!currentUser.RoleIds.Any(p => _roles.Contains((RoleType)p)))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}
