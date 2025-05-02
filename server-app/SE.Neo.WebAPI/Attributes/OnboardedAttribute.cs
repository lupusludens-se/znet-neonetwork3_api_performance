
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.User;
using System.Net;

namespace SE.Neo.WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OnboardedAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
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
            if (currentUser.Status != UserStatus.Onboard)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
        }
    }
}
