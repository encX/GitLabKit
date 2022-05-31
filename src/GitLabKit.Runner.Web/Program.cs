using GitLabKit.Runner.Core.Configs;
using GitLabKit.Runner.Core.Repositories;
using GitLabKit.Runner.Core.Services;
using GitLabKit.Runner.Web.Startup;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.Extensions;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment.EnvironmentName;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env}.json", true)
    .AddJsonFile($"appsettings.Secrets.json", true)
    .AddEnvironmentVariables()
    .Build();

var logger = LoggerConfigurator.GetLogConfig(configuration, env).CreateLogger();

builder.Host.UseSerilog(logger);
Log.Logger = logger;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "GitLabKit.Runner", Version = "v1"});
    // Set the comments path for the Swagger JSON and UI.
    c.AddAutoRestCompatible();
    c.UseInlineDefinitionsForEnums();
    c.MakeValueTypePropertiesRequired();
    c.DefineOperationIdFromControllerNameAndActionName();
    c.EnableAnnotations();
});

var connectionsConfig = configuration.GetSection(nameof(Connections));
builder.Services.Configure<Connections>(connectionsConfig);
builder.Services.Configure<ApplicationInsights>(configuration.GetSection(nameof(ApplicationInsights)));

var secretsConfig = configuration.GetSection(nameof(Secrets));
builder.Services.Configure<Secrets>(secretsConfig);

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationInsightsTelemetry().EnrichAppInsightsData();
builder.Services.AddSingleton<ITelemetryInitializer, ApplicationInsightsRoleNameInitializer>();
builder.Services.AddCache(connectionsConfig, secretsConfig);
builder.Services.AddSingleton<IGitLabRepository, GitLabRepository>();
builder.Services.AddSingleton<IGitLabGroupService, GitLabGroupService>();
builder.Services.AddSingleton<IGitLabRunnerService, GitLabRunnerService>();

Mapper.Map();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app
    .UseSerilogRequestLogging()
    .UseStaticFiles()
    .UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    })
    .UseSwagger()
    .UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GitLabKit.Runner");
        c.RoutePrefix = "swagger";
    });

app.MapFallbackToFile("index.html");

app.Run();