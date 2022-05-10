using GitLabKit.Runner.Core.Models;

namespace GitLabKit.Runner.Mock;

public static class Generator
{
    public static Project GetMockProject() => new()
    {
        Id = 12345,
        Name = "project",
        PathWithNamespace = "path/to/project"
    };

    public static Core.Models.Runner GetMockRunner(int runnerId) =>
        new()
        {
            Id = runnerId,
            Description = $"test-runner-{runnerId}",
            IpAddress = $"10.1.1.{runnerId % 255}",
            Active = runnerId % 7 != 0,
            Online = runnerId % 15 != 0,
            ContactedAt = DateTime.Now,
            TagList = new List<string>{runnerId < 2010 ? "windows" : "linux", $"tag-{runnerId % 3 + 1}"}
        };

    public static Job GetMockJob(int jobId, string status)
    {
        var startTime = DateTime.Now.Subtract(TimeSpan.FromSeconds(new Random().Next(15 * 60, 120 * 60)));
        var finishTime = DateTime.Now.Subtract(TimeSpan.FromSeconds(new Random().Next(0, 15 * 60)));
        
        return new Job
        {
            Id = jobId,
            Status = status,
            Name = "build",
            Ref = $"merge/{new Random().Next(1, 9999)}",
            StartedAt = startTime,
            Duration = (finishTime - startTime).TotalSeconds,
            WebUrl = "https://gitlab.com",
            FinishedAt = status == "running" ? null : finishTime,
            Project = GetMockProject()
        };
    }
}