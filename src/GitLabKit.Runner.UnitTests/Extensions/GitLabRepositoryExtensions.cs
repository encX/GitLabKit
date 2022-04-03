using System.Collections.Generic;
using System.Threading.Tasks;
using GitLabKit.Runner.Core.Models;
using GitLabKit.Runner.Core.Repositories;
using NSubstitute;

namespace GitLabKit.Runner.UnitTests.Extensions;

public static class GitLabRepositoryExtensions
{
    public static IGitLabRepository WhenGetGroupRunnersReturns(this IGitLabRepository repo, IEnumerable<Core.Models.Runner> runners)
    {
        repo.GetGroupRunners(Arg.Any<int>())
            .Returns(Task.FromResult(runners));

        return repo;
    }
    
    public static IGitLabRepository WhenGetRunnerCurrentJobReturnsForRunnerId(this IGitLabRepository repo, int runnerId, IEnumerable<Job> jobs)
    {
        repo.GetRunnerCurrentJobs(Arg.Is(runnerId))
            .Returns(Task.FromResult(jobs));

        return repo;
    }
    
    public static IGitLabRepository WhenGetSingleRunnerReturnsForRunnerId(this IGitLabRepository repo, int runnerId, Core.Models.Runner runner)
    {
        repo.GetSingleRunner(Arg.Is(runnerId))
            .Returns(Task.FromResult(runner));

        return repo;
    }
    
    public static IGitLabRepository WhenSetRunnerActiveStatusReturns(this IGitLabRepository repo, bool result)
    {
        repo.SetRunnerActiveStatus(Arg.Any<int>(), Arg.Any<bool>())
            .Returns(Task.FromResult(result));

        return repo;
    }
    
    public static IGitLabRepository WhenInvalidateRunnerCacheCompleted(this IGitLabRepository repo)
    {
        repo.InvalidateRunnerCache(Arg.Any<int>()).Returns(Task.CompletedTask);

        return repo;
    }
}