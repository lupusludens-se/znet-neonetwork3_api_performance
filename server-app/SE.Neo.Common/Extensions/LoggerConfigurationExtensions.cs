using Microsoft.Extensions.Configuration;
using SE.Neo.Common.Enums;
using Serilog;
using Serilog.Events;
using Serilog.Filters;
using Serilog.Sinks.MSSqlServer;
using System.Data;

namespace SE.Neo.Common.Extensions
{
    public static class LoggerConfigurationExtensions
    {
        public static ILogger BuildLogger(this IConfiguration configuration, LoggerSourceType sourceType, EnvironmentEnum environment)
        {
            ColumnOptions columnOptions = new ColumnOptions();
            columnOptions.Store.Remove(StandardColumn.MessageTemplate);
            columnOptions.AdditionalColumns = new List<SqlColumn>()
                            {
                                new SqlColumn { PropertyName = "UserName", ColumnName = "User_Name", DataType = SqlDbType.NVarChar, DataLength = 100 },
                                new SqlColumn { PropertyName = "Source", ColumnName = "Source", DataType = SqlDbType.Int}
                            };
            columnOptions.TimeStamp.ColumnName = "Timestamp";
            columnOptions.TimeStamp.ConvertToUtc = true;
            columnOptions.Properties.DataType = System.Data.SqlDbType.Xml;

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
                    restrictedToMinimumLevel: (environment == EnvironmentEnum.Prod || environment == EnvironmentEnum.PreProd) ? LogEventLevel.Error : LogEventLevel.Information)
                .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore.Query"))
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Source", (int)sourceType)
                .CreateLogger();
        }
    }
}
