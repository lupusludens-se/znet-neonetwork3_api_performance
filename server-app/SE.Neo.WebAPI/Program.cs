using Azure.Identity;
using FileTypeChecker;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Infrastructure.Models.FileTypes;
using SE.Neo.WebAPI.Extensions;
using SE.Neo.WebAPI.Filters;
using SE.Neo.WebAPI.Models.Shared;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using LoggerConfigurationExtensions = SE.Neo.WebAPI.Extensions.LoggerConfigurationExtensions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddApplicationInsightsTelemetry();

    string environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "dev";
    Log.Information($"Environment Set: {environment}");
    builder.Configuration.AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{environment}.json");

    // Adds Microsoft Identity platform (AAD v2.0) support to protect this Api
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(options =>
            {
                builder.Configuration.Bind("AzureAdB2C", options);
                options.TokenValidationParameters.NameClaimType = "name";
            },
            options => { builder.Configuration.Bind("AzureAdB2C", options); }
    );

    // Creating policies that wraps the authorization requirements
    builder.Services.AddAuthorization();

    builder.Services.AddMvc(options =>
    {
        options.Filters.Add<OperationCancelledExceptionFilter>();
        options.Filters.Add<DecodeFilter>();
        options.Filters.Add<StringTrimmerFilter>();
    });


    builder.Configuration.AddAzureKeyVault(
        new Uri(builder.Configuration.GetSection("AzureKeyVault")["KeyVaultUrl"]),
        new ChainedTokenCredential(
            new DefaultAzureCredential(new DefaultAzureCredentialOptions()
            {
                VisualStudioTenantId = builder.Configuration.GetSection("AzureKeyVault")["TenantId"],
                ManagedIdentityClientId = builder.Configuration.GetSection("AzureKeyVault")["ClientId"]
            })));

    var connectionString =
# if DEBUG
        builder.Configuration.GetValue<string>("ConnectionStrings:LocalConnection") ??
#endif
        builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

    builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        loggerConfiguration
            .ConfigureBaseLogging(context.Configuration, connectionString, LoggerSourceType.Api, environment.GetValueFromDescription<EnvironmentEnum>())
            .AddApplicationInsightsLogging(services, context.Configuration)
    );

    builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddMemoryCache();

    builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context => new UnprocessableEntityObjectResult(new ValidationResponse(context.ModelState));
    }).AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    }).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

    // Allowing CORS for all domains and methods for the purpose of the sample
    // In production, modify this with the actual domains you want to allow
    builder.Services.AddCors(o => o.AddPolicy("default", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    }));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {

        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "NEO API",
            Version = "v1"
        });
        options.DescribeAllParametersInCamelCase();

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type=ReferenceType.SecurityScheme,
                        Id="Bearer"
                    }
                },
                new string[]{}
            }
        });

        options.DocumentFilter<CustomEnvironmentFilter>();
        options.DocumentFilter<JsonPatchDocumentFilter>();

        // get xml from all generated assemblies
        List<string> xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly).ToList();
        xmlFiles.ForEach(xmlFile =>
        {
            options.IncludeXmlComments(xmlFile);
            options.SchemaFilter<EnumTypesSchemaFilter>(xmlFile);
        });

        options.DocumentFilter<TypesDocumentFilter>();

    });
    builder.Services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add
                ("/Templates/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add
                ("/Templates/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add
                ("/Templates/Shared/{0}" + RazorViewEngine.ViewExtension);
            });

    builder.Services.AddAutoMapper(
        typeof(SE.Neo.Core.Mapping.ProjectProfile),
        typeof(SE.Neo.WebAPI.Mapping.UserApiProfile),
        typeof(SE.Neo.Infrastructure.Mapping.SolarInfrastructureProfile));

    if (environment == "local")
    {
        builder.Services.AddDistributedMemoryCache();
    }
    else
    {
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetValue<string>("AzureRedisCache:ConnectionString");
            options.InstanceName = builder.Configuration.GetSection("AzureRedisCache")["InstanceName"];
        });
    }

    // Register configs
    builder.Services.AddConfig(builder.Configuration);

    builder.Services.AddDependencies();

    FileTypeValidator.RegisterCustomTypes(typeof(XmlFileType).Assembly);
    FileTypeValidator.RegisterCustomTypes(typeof(SvgFileType).Assembly);


    // Configure services
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.Cookie.HttpOnly = true;

    });

    var app = builder.Build();

    var basePath = $"/zeigonetwork/{environment}";
    Log.Information($"Base Path Set: {basePath}");
    app.UsePathBase(new PathString(basePath));
    app.UseRouting();

    if (environment != "prod")
    {
        Console.WriteLine("Setting Swagger Path");
        app.UseSwagger(c =>
        {
            c.RouteTemplate = basePath.Remove(0) + "api/swagger/{documentName}/swagger.json";
            c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
            {
                swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"https://{httpReq.Host.Value}{basePath}" } };
            });
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint(basePath + "/api/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = "api";
        });
    }
    if (environment == "local")
    {
        app.UseMigrationsEndPoint();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ApplicationContext>();
        }
    }

    app.UseStatusCodePages();

    app.UseCors("default");
    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    // it's used to provide for log record information about user
    app.UseSerilogUserNameEnricher();

    // the next 2 lines are used to customize information about request within Property field of a log record
    app.UseRequestResponseLogging();

    app.UseExceptionHandler(c => c.Run(async context =>
        {
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var exception = exceptionHandlerPathFeature?.Error;
            string exceptionMessage = exception is CustomException ? exception.Message : CoreErrorMessages.UnexpectedError;
            context.Response.StatusCode = exception is CustomException ? StatusCodes.Status200OK : StatusCodes.Status500InternalServerError;
            context.Response.ContentType = Application.Json;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { errorMessages = exceptionMessage }));
        })
    );

    app.UseSerilogRequestLogging(opts =>
    {
        opts.EnrichDiagnosticContext = LoggerConfigurationExtensions.EnrichFromRequest;
        opts.GetLevel = LoggerConfigurationExtensions.GetLevel;
    });

    app.Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
