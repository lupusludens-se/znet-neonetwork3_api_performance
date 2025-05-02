using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Core.Data;
using SE.Neo.Core.Services;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.EmailAlertSender;
using SE.Neo.EmailAlertSender.Interfaces;
using SE.Neo.EmailTemplates.Models;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services;
using SE.Neo.Infrastructure.Services.Interfaces;
using Serilog;
using System.Diagnostics;

DateTime _datetime = DateTime.UtcNow;
try
{
    #region Setup

    string environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "local";
    Console.WriteLine($"Environment Set: {environment}");

    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json", false)
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
    Log.Logger = configuration.BuildLogger(LoggerSourceType.EmailAlertSender, environment.GetValueFromDescription<EnvironmentEnum>());
    var connectionString =
# if DEBUG
        configuration.GetValue<string>("ConnectionStrings:LocalConnection") ??
#endif
        configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

    IHostBuilder builder = Host.CreateDefaultBuilder(args);
    builder.ConfigureServices(services =>
    {
        services.AddConfig(configuration);
        var emailTemplateAssembly = typeof(CompleteProfileEmailTemplatedModel).Assembly;
        var emailTemplateAssemblyPart = new CompiledRazorAssemblyPart(emailTemplateAssembly);
        services.AddMvc().PartManager.ApplicationParts.Add(emailTemplateAssemblyPart);
        services.Configure<RazorViewEngineOptions>(options =>
        {
            options.ViewLocationFormats.Clear();
            options.ViewLocationFormats.Add
            ("/Templates/{0}" + RazorViewEngine.ViewExtension);
            options.ViewLocationFormats.Add
            ("/Templates/{1}/{0}" + RazorViewEngine.ViewExtension);
            options.ViewLocationFormats.Add
            ("/Templates/Shared/{0}" + RazorViewEngine.ViewExtension);
        });
        var listener = new DiagnosticListener("Microsoft.AspNetCore");
        services.AddSingleton<DiagnosticListener>(listener);
        services.AddSingleton<DiagnosticSource>(listener);
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));
        services.AddAutoMapper(typeof(SE.Neo.EmailAlertSender.Mapping.EmailAlertProfile));
        services.AddScoped<IFormFileValidationService, FormFileValidationService>();
        services.AddSingleton<BlobTypesFilesLimitationsConfig>();
        services.AddSingleton<IRazorViewEngine, RazorViewEngine>();
        services.AddScoped<IActionContextAccessor, ActionContextAccessor>();

        services.AddScoped<IAzureStorageBlobService, AzureStorageBlobService>();
        services.AddScoped<IBlobService, BlobService>();
        services.AddScoped<IBlobServicesFacade, BlobServicesFacade>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailAlertSendService, EmailAlertSendService>();
        services.AddScoped<IRenderEmailTemplateService, RenderEmailTemplateService>();
        services.AddScoped<ICurrentUserResolverService, CurrentUserResolverService>();
        services.AddScoped<ISendgridService, SendgridService>();
        services.AddHttpClient<ISendgridService, SendgridService>();
        services.AddScoped<IUnsubscribeEmailService, UnsubscribeEmailService>();
    });
    builder.UseSerilog();

    #endregion Setup

    var app = builder.Build();
    Log.Information($"Started service {_datetime}");

    using (var scope = app.Services.CreateScope())
    {
        var emailAlertSendService = scope.ServiceProvider.GetRequiredService<IEmailAlertSendService>();
        var httpContext = new DefaultHttpContext { RequestServices = scope.ServiceProvider };
        var routeData = new RouteData();
        routeData.Values.Add("controller", "Home");
        var actionContext = new ActionContext(httpContext, routeData, new ActionDescriptor());
        await emailAlertSendService.SendEmailsAsync(actionContext);
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information($"Shut down complete for the service started at  {_datetime}");
    Log.CloseAndFlush();
}