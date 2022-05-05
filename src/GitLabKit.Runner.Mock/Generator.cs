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
            Active = runnerId % 2 == 0,
            Online = runnerId % 3 == 0,
            ContactedAt = DateTime.Now
        };

    public static Job GetMockJob(int seed, string status) =>
        new()
        {
            Id = seed + 1,
            Status = status,
            Name = "build",
            Ref = $"merge/{seed << 1 % 100}",
            StartedAt = DateTime.Now.Subtract(TimeSpan.FromMinutes(seed % 10 + 5)),
            Duration = (seed << 2) % 500 << 1,
            WebUrl = "https://gitlab.com",
            FinishedAt = status == "running" ? DateTime.Now.Subtract(TimeSpan.FromMinutes(seed % 10 + 3)) : null,
            Project = GetMockProject()
        };
}