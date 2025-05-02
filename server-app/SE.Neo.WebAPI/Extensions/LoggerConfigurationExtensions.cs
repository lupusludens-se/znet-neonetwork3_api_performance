using Microsoft.ApplicationInsights.Extensibility;
using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Attributes;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace SE.Neo.WebAPI.Extensions
{
    internal static class LoggerConfigurationExtensions
    {
        internal static LoggerConfiguration ConfigureBaseLogging(this LoggerConfiguration loggerConfiguration, IConfiguration configuration, string connectionString, LoggerSourceType source, EnvironmentEnum environment)
        {
            ColumnOptions columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.AdditionalColumns = new List<SqlColumn>() {
                new SqlColumn { PropertyName = "UserName", ColumnName = "User_Name", DataType = SqlDbType.NVarChar, DataLength = 100 },
                new SqlColumn { PropertyName = "Source", ColumnName = "Source", DataType = SqlDbType.Int}
            };

            columnOptions.TimeStamp.ColumnName = "Timestamp";
            columnOptions.TimeStamp.ConvertToUtc = true;

            columnOptions.Properties.DataType = SqlDbType.Xml;

            return loggerConfiguration
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "Log_Event",
                        AutoCreateSqlTable = true
                    },
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: (environment == EnvironmentEnum.Prod || environment == EnvironmentEnum.PreProd) ? LogEventLevel.Error : LogEventLevel.Information
                )
                .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore.Query"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware"))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Source", (int)source)
                .ReadFrom.Configuration(configuration);
        }

        internal static LoggerConfiguration AddApplicationInsightsLogging(this LoggerConfiguration loggerConfiguration, IServiceProvider services, IConfiguration configuration)
        {
            if (!string.IsNullOrWhiteSpace(configuration.GetSection("AppInsights")["InstrumentationKey"]))
            {
                loggerConfiguration.WriteTo.ApplicationInsights(
                    services.GetRequiredService<TelemetryConfiguration>(),
                    TelemetryConverter.Traces);
            }

            return loggerConfiguration;
        }

        private static bool IsVerbose(HttpContext ctx)
        {
            Endpoint? endpoint = ctx.GetEndpoint();
            if (endpoint is not null)
                return endpoint.Metadata.Any(m => m.GetType() == typeof(ExcludeLoggingAttribute));

            return false;
        }

        internal static LogEventLevel GetLevel(HttpContext ctx, double _, Exception ex)
        {
            if (ex == null)
            {
                if (ctx.Response.StatusCode <= 499)
                {
                    if (IsVerbose(ctx))
                        return LogEventLevel.Verbose;

                    return LogEventLevel.Information;
                }

                return LogEventLevel.Error;
            }

            return LogEventLevel.Error;
        }

        internal static async void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;
            string requestBodyPayload = string.Empty;
            try
            {
                requestBodyPayload = await ReadRequestBody(httpContext.Request);
            }
            catch
            {
                // do nothing
            }

            diagnosticContext.Set("RequestBody", requestBodyPayload);
            string responseBodyPayload = string.Empty;
            try
            {
                responseBodyPayload = await ReadResponseBody(httpContext.Response);
            }
            catch
            {
                // do nothing
            }
            diagnosticContext.Set("ResponseBody", responseBodyPayload);

            // Set all the common properties available for every request
            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            // Only set it if available.You're not sending sensitive data in a querystring right?!
            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            // Set the content-type of the Response at this point
            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            var endpoint = httpContext.GetEndpoint();
            if (endpoint != null)
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }
        }

        private static async Task<string> ReadResponseBody(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return responseBody;
        }

        private static async Task<string> ReadRequestBody(HttpRequest request)
        {
            request.Body.Seek(0, SeekOrigin.Begin);
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);

            return requestBody;
        }
    }
}