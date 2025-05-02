using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using SE.Neo.Common.Enums;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace SE.Neo.CompanyLogoExporter
{
    public static class ServiceProviderExtensions
    {
        public static ILogger BuildLogger(this IServiceProvider serviceProvider, IConfiguration configuration, EnvironmentEnum environment)
        {
            ColumnOptions columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.AdditionalColumns = new List<SqlColumn>()
            {
                new SqlColumn { PropertyName = "UserName", ColumnName = "User_Name", DataType = SqlDbType.NVarChar, DataLength = 100 }
            };
            columnOptions.TimeStamp.ColumnName = "Timestamp";
            columnOptions.TimeStamp.ConvertToUtc = true;
            columnOptions.Properties.DataType = SqlDbType.Xml;

            var connectionString =
#if DEBUG
                configuration.GetValue<string>("ConnectionStrings:LocalConnection") ??
#endif
                configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.MSSqlServer(
                    connectionString,
                    new MSSqlServerSinkOptions
                    {
                        TableName = "Log_Event",
                        AutoCreateSqlTable = true
                    },
                    columnOptions: columnOptions,
                    restrictedToMinimumLevel: environment == EnvironmentEnum.Prod ? LogEventLevel.Error : LogEventLevel.Information)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}