using GitLabKit.Runner.Core.Configs;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Options;

namespace GitLabKit.Runner.Web.Startup;

public class ApplicationInsightsRoleNameInitializer : ITelemetryInitializer
{
    private readonly string _appName;
    
    public ApplicationInsightsRoleNameInitializer(IOptions<ApplicationInsights> ai)
    {
        _appName = ai.Value.CloudRole;
    }
    
    public void Initialize(ITelemetry telemetry)
    {
        telemetry.Context.Cloud.RoleName = _appName;
    }
}