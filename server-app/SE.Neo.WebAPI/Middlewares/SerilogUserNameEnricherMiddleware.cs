using SE.Neo.WebAPI.Models.User;
using Serilog.Context;

namespace SE.Neo.WebAPI.Middlewares
{
    public class SerilogUserNameEnricherMiddleware
    {
        readonly RequestDelegate _next;

        public SerilogUserNameEnricherMiddleware(RequestDelegate next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            string? userName = ((UserModel?)httpContext.Items["User"])?.Username;

            // Push the user name into the log context so that it is included in all log entries
            using (LogContext.PushProperty("UserName", userName))
            {
                await _next(httpContext);
            }
        }

    }
}
