namespace SE.Neo.WebAPI.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            HttpRequestRewindExtensions.EnableBuffering(context.Request);
            // Read and log response body data Copy a pointer to the original response body stream
            var originalResponseBodyStream = context.Response.Body;

            // Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                // ...and use that for the temporary response body
                context.Response.Body = responseBody;

                // Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);

                // Copy the contents of the new memory stream (which contains the response) to the
                // original stream, which is then returned to the client.
                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalResponseBodyStream);
            }
        }
    }
}