using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.DataSync;
using Serilog;

try
{
    var environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "local";
    Console.WriteLine($"Environment Set: {environment}");

    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.{environment}.json", false)
        .Build();

    configuration = new ConfigurationBuilder()
    .AddConfiguration(configuration)
    .AddAzureKeyVault(
        new Uri(configuration.GetSection("AzureKeyVault")["KeyVaultUrl"]),
        new ChainedTokenCredential(
            new DefaultAzureCredential(new DefaultAzureCredentialOptions()
            {
                VisualStudioTenantId = configuration.GetSection("AzureKeyVault")["TenantId"],
                ManagedIdentityClientId = configuration.GetSection("AzureKeyVault")["ClientId"]
            })))
    .Build();

    Log.Logger = configuration.BuildLogger(LoggerSourceType.DataSync, environment.GetValueFromDescription<EnvironmentEnum>());

    var builder = Host.CreateDefaultBuilder(args);

    builder.ConfigureServices(services =>
    {
        services.AddConfig(configuration);
        services.AddHttpClient<DataSyncService>();
    });

    builder.UseSerilog();
    Log.Logger.Information("Building App.");
    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        var syncService = scope.ServiceProvider.GetRequiredService<DataSyncService>();
        await syncService.StartSyncAsync(args);
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception.");
}
finally
{
    Log.Information("Shut down complete.");
    Log.CloseAndFlush();
}