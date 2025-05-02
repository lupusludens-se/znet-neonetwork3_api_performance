
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.User;
using System.Net;

namespace SE.Neo.WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionsAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly PermissionType[] _permissions;

        /// <summary>  
        /// Initializes a new instance of the <see cref="PermissionsAttribute"/> class.  
        /// </summary>  
        /// <param name="permissions">The permissions required to access the resource.</param>  
        public PermissionsAttribute(params PermissionType[] permissions)
        {
            _permissions = permissions;
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
            if (!currentUser.PermissionIds.Any(p => _permissions.Contains((PermissionType)p)))
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}
