using System;
using GitLabKit.Runner.Core.Configs;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace GitLabKit.Runner.Web.Startup;

public static class LoggerConfigurator
{
    public static LoggerConfiguration GetLogConfig(IConfiguration configuration, string env)
    {
        var assemName = typeof(Program).Assembly.GetName();

        var logConfig = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .WriteTo.Console()
            .Enrich.WithProperty("MachineName", Environment.MachineName)
            .Enrich.WithProperty("Version", assemName.Version?.ToString())
            .Enrich.WithProperty("AssemblyName", assemName.Name)
            .Enrich.WithProperty("EnvironmentName", env)
            .Enrich.WithProperty("OS", Environment.OSVersion.Platform)
            .Enrich.FromLogContext();

        var logTarget = configuration.GetSection(nameof(LogTargets)).Get<LogTargets>();

        if (!string.IsNullOrEmpty(logTarget.Seq))
        {
            logConfig = logConfig.WriteTo.Seq(logTarget.Seq);
        }
        
        return logConfig;
    }
}