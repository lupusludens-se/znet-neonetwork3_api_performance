using Azure.Identity;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Media;
using SE.Neo.CompanyLogoExporter;
using SE.Neo.CompanyLogoExporter.Models;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services;
using SE.Neo.Infrastructure.Services.Interfaces;
using Serilog;
using System.Globalization;

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

    var connectionString =
# if DEBUG
        configuration.GetValue<string>("ConnectionStrings:LocalConnection") ??
#endif
        configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

    IHostBuilder builder = Host.CreateDefaultBuilder(args);
    builder.ConfigureServices(services =>
    {
        services.AddConfig(configuration);

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));
        services.AddAutoMapper(typeof(SE.Neo.Core.Mapping.BlobProfile));
        services.AddScoped<IFormFileValidationService, FormFileValidationService>();
        services.AddSingleton<BlobTypesFilesLimitationsConfig>();
        services.AddScoped<ICurrentUserResolverService, CurrentUserResolverService>();
        services.AddScoped<IAzureStorageBlobService, AzureStorageBlobService>();
        services.AddScoped<IBlobService, BlobService>();
        services.AddScoped<IBlobServicesFacade, BlobServicesFacade>();
    });

    #endregion

    var app = builder.Build();
    using (var scope = app.Services.CreateScope())
    {
        Log.Logger = scope.ServiceProvider.BuildLogger(configuration, environment.GetValueFromDescription<EnvironmentEnum>());
        Log.Logger.Information("Company logo exporter started.");

        TextReader input = Console.In;

        // If an argument is specified, read from a file, otherwise from the pipe.
        if (args.Any())
        {
            string path = args[0];
            if (File.Exists(path))
                input = File.OpenText(path);
        }

        var blobServicesFacade = scope.ServiceProvider.GetRequiredService<IBlobServicesFacade>();
        var formFileValidation = scope.ServiceProvider.GetRequiredService<IFormFileValidationService>();
        var blobTypesFilesLimitationsConfig = (BlobTypesFilesLimitationsConfig)scope.ServiceProvider.GetService(typeof(BlobTypesFilesLimitationsConfig))!;

        using (var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>())
        using (var csvReader = new CsvReader(input, CultureInfo.InvariantCulture))
        using (var httpClient = new HttpClient())
            while (csvReader.Read())
            {
                CompanyExportRequest companyExportRequest = csvReader.GetRecord<CompanyExportRequest>();
                Company? company = await context.Companies.FirstOrDefaultAsync(c => c.Name == companyExportRequest.Name);

                if (company == null)
                {
                    Log.Logger.Warning($"No company entity with {companyExportRequest.Name} name is found in the databade.");
                    continue;
                }

                if (company.ImageLogo != null)
                {
                    Log.Logger.Warning($"Company {company.Name} already have logo uploaded by the name {company.ImageLogo}.");
                    continue;
                }

                using (HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(companyExportRequest.CompanyLogoURL))
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        BlobDTO blobDTO;
                        using (Stream stream = await httpResponseMessage.Content.ReadAsStreamAsync())
                        {
                            string imageName = companyExportRequest.CompanyLogoURL.Split(@"/").Last();
                            IFormFile formFile = new FormFile(
                                stream,
                                0,
                                stream.Length,
                                companyExportRequest.CompanyLogoURL,
                                imageName);
                            string? errMesg;
                            if (!formFileValidation.IsValid(formFile, blobTypesFilesLimitationsConfig[BlobType.Companies], out errMesg))
                            {
                                Log.Logger.Warning($"Requested resource {companyExportRequest.CompanyLogoURL} is not image.");
                                continue;
                            }
                            stream.Position = 0;

                            BlobBaseDTO blobBaseDTO = new BlobBaseDTO { Name = imageName, ContainerName = BlobType.Companies.ToString() };
                            blobDTO = await blobServicesFacade.CreateBlobAsync(stream, blobBaseDTO);
                        }

                        company.ImageLogo = blobDTO.Name;
                        await context.SaveChangesAsync();
                    }
                    else
                    {
                        Log.Logger.Warning($"Request for resource by {companyExportRequest.CompanyLogoURL} received response {httpResponseMessage.StatusCode} - {httpResponseMessage.ReasonPhrase}.");
                        continue;
                    }

                Log.Logger.Information($"Logo for {company.Name} company successfully uploaded from {companyExportRequest.CompanyLogoURL}.");
            }

        Log.Logger.Information("Company logo exporter finished.");
    }
}
catch (Exception ex)
{
    Log.Logger.Error(ex, ex.Message);
}