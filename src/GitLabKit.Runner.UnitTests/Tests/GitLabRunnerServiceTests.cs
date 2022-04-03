using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitLabKit.Runner.Core.Models;
using GitLabKit.Runner.Core.Services;
using GitLabKit.Runner.UnitTests.Extensions;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace GitLabKit.Runner.UnitTests.Tests;

[TestFixture]
public class GitLabRunnerServiceTests
{
    private readonly Core.Models.Runner _mockRunner1 = new() {Id = 1, TagList = new[] {"foo"}};
    private readonly Core.Models.Runner _mockRunner2 = new() {Id = 2, TagList = new[] {"bar"}};

    private readonly List<Core.Models.Runner> _mockRunners = new List<Core.Models.Runner>
    {
        new() {Id = 1},
        new() {Id = 2},
    };

    private readonly Job _mockJob = new();

    [Test]
    public async Task GetGroupRunners_WhenCallWithGroupId_ShouldGetGroupRunners()
    {
        const int groupId = 1234;
        var repo = Given.GitLabRepository
            .WhenGetGroupRunnersReturns(_mockRunners)
            .WhenGetSingleRunnerReturnsForRunnerId(1, _mockRunner1)
            .WhenGetRunnerCurrentJobReturnsForRunnerId(1, new []{_mockJob})
            .WhenGetSingleRunnerReturnsForRunnerId(2, _mockRunner2)
            .WhenGetRunnerCurrentJobReturnsForRunnerId(2, new List<Job>());

        var service = new GitLabRunnerService(repo);

        var runners = (await service.GetGroupRunners(groupId)).ToList();

        runners.Count.ShouldBe(_mockRunners.Count);
        runners.First(r => r.Id == 1).TagList.ShouldBe(_mockRunner1.TagList);
        runners.First(r => r.Id == 2).TagList.ShouldBe(_mockRunner2.TagList);
        runners.First(r => r.Id == 1).CurrentJob.ShouldHaveSingleItem();
        runners.First(r => r.Id == 2).CurrentJob.ShouldBeEmpty();

        await repo.Received(1).GetGroupRunners(groupId);
        await repo.Received(_mockRunners.Count).GetRunnerCurrentJobs(Arg.Any<int>());
        await repo.Received(_mockRunners.Count).GetSingleRunner(Arg.Any<int>());
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task SetRunnerActiveStatus_WhenCalled_ShouldPassToRepoAndInvalidateCache(bool resultFromRepo)
    {
        const int runnerId = 12345;
        var repo = Given.GitLabRepository
            .WhenSetRunnerActiveStatusReturns(resultFromRepo)
            .WhenInvalidateRunnerCacheCompleted();

        var service = new GitLabRunnerService(repo);

        var result = await service.SetRunnerActiveStatus(runnerId, true);

        result.ShouldBe(resultFromRepo);

        await repo.Received(1).SetRunnerActiveStatus(runnerId, Arg.Any<bool>());
        await repo.Received(1).InvalidateRunnerCache(runnerId);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task BulkSetRunnerActiveStatus_WhenCalled_ShouldPassToRepoAndInvalidateCaches(bool resultFromRepo)
    {
        const int runnerId = 12345;
        var repo = Given.GitLabRepository
            .WhenSetRunnerActiveStatusReturns(resultFromRepo)
            .WhenInvalidateRunnerCacheCompleted();

        var request = new BulkRunnerActiveStatus
        {
            Runners = new Dictionary<int, bool>
            {
                {1, false},
                {2, true}
            }
        };

        var service = new GitLabRunnerService(repo);

        await service.BulkSetRunnerActiveStatus(request);
        
        await repo.Received(1).SetRunnerActiveStatus(1, false);
        await repo.Received(1).SetRunnerActiveStatus(2, true);
        await repo.Received(1).InvalidateRunnerCache(1);
        await repo.Received(1).InvalidateRunnerCache(2);
    }
}