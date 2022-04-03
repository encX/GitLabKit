using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitLabKit.Runner.Core.Models;
using GitLabKit.Runner.Core.Repositories;

namespace GitLabKit.Runner.Core.Services;

public interface IGitLabRunnerService
{
    Task<IEnumerable<Models.Runner>> GetGroupRunners(int groupId);
    Task<Models.Runner> GetSingleRunner(int runnerId);
    Task<bool> SetRunnerActiveStatus(int runnerId, bool isActive);
    Task BulkSetRunnerActiveStatus(BulkRunnerActiveStatus status);
    Task DeleteRunner(int runnerId);
    Task BulkDeleteRunner(IEnumerable<int> runnerIds);
    Task<IEnumerable<Job>> GetRunnerJobHistory(int runnerId);
}

public class GitLabRunnerService : IGitLabRunnerService
{
    private readonly IGitLabRepository _repository;
    
    public GitLabRunnerService(IGitLabRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Models.Runner>> GetGroupRunners(int groupId)
    {
        var runners = await _repository.GetGroupRunners(groupId);
        var runnerJobTasks = runners
            .Select(async r =>
            {
                var currentJob = await _repository.GetRunnerCurrentJobs(r.Id);
                var runner = await _repository.GetSingleRunner(r.Id);

                r.CurrentJob = currentJob;
                r.TagList = runner.TagList;
                
                // overrides in case of status changes
                r.Active = runner.Active;

                return r;
            })
            .ToList();

        await Task.WhenAll(runnerJobTasks);

        return runnerJobTasks.Select(t => t.Result).OrderByDescending(r => r.Id);
    }

    public async Task<Models.Runner> GetSingleRunner(int runnerId) => await _repository.GetSingleRunner(runnerId);

    public async Task<bool> SetRunnerActiveStatus(int runnerId, bool isActive)
    {
        var status = await _repository.SetRunnerActiveStatus(runnerId, isActive);
        await _repository.InvalidateRunnerCache(runnerId);

        return status;
    }

    public async Task BulkSetRunnerActiveStatus(BulkRunnerActiveStatus status)
    {
        var statusChangeTasks = status.Runners.Keys.Select(id => _repository.SetRunnerActiveStatus(id, status.Runners[id]));
        var invalidateCacheTasks = status.Runners.Keys.Select(runnerId => _repository.InvalidateRunnerCache(runnerId));
        await Task.WhenAll(statusChangeTasks);
        await Task.WhenAll(invalidateCacheTasks);
    }

    public async Task DeleteRunner(int runnerId) => await _repository.DeleteRunner(runnerId);

    public async Task BulkDeleteRunner(IEnumerable<int> runnerIds) => await Task.WhenAll(runnerIds.Select(_repository.DeleteRunner));

    public async Task<IEnumerable<Job>> GetRunnerJobHistory(int runnerId) => (await _repository.GetRunnerJobs(runnerId)).OrderByDescending(j => j.StartedAt);
}