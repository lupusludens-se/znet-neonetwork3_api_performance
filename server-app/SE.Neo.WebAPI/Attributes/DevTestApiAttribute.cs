using Microsoft.AspNetCore.Mvc.Filters;

namespace SE.Neo.WebAPI.Attributes
{
    // using to track api only for dev and test purpose
    public class DevTestApiAttribute : ActionFilterAttribute
    {
    }
}
