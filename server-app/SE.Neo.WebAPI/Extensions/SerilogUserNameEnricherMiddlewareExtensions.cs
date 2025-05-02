using SE.Neo.WebAPI.Middlewares;

namespace SE.Neo.WebAPI.Extensions
{
    public static class SerilogUserNameEnricherMiddlewareExtensions
    {
        public static IApplicationBuilder UseSerilogUserNameEnricher(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogUserNameEnricherMiddleware>();
        }
    }
}
