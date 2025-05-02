using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.User;
using System.Net;

namespace SE.Neo.WebAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ActiveAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public bool _skipAuthorize { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveAttribute"/> class.
        /// </summary>
        /// <param name="skipAuthorize">Indicates whether to skip authorization.</param>
        public ActiveAttribute(bool skipAuthorize = false)
        {
            _skipAuthorize = skipAuthorize;
        }

        /// <summary>
        /// Called to check if the user is authorized to access the resource.
        /// </summary>
        /// <param name="context">The authorization filter context.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ControllerActionDescriptor? descriptor = context?.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor == null)
            {
                return;
            }

            var activeAttribute = descriptor.MethodInfo.GetCustomAttributes(typeof(ActiveAttribute), inherit: true).FirstOrDefault() as ActiveAttribute;

            if (activeAttribute == null || !activeAttribute._skipAuthorize)
            {
                var currentUser = (UserModel?)context.HttpContext.Items["User"];
                if (currentUser == null)
                {
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Unauthorized);
                    return;
                }
                if (currentUser.Status != UserStatus.Active)
                {
                    var logger = context.HttpContext.RequestServices.GetService(typeof(ILogger<ActiveAttribute>)) as ILogger<ActiveAttribute>;
                    logger.LogWarning(string.Format("User {0} is not Active", currentUser.Username));
                    context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                    return;
                }
            }
        }
    }
}
