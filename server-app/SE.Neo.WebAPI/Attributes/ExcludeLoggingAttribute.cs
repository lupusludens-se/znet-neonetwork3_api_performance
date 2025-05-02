using Microsoft.AspNetCore.Mvc.Filters;

namespace SE.Neo.WebAPI.Attributes
{
    /// <summary>
    /// Attribute to exclude logging for specific actions or controllers.
    /// </summary>
    public class ExcludeLoggingAttribute : ActionFilterAttribute { }
}
