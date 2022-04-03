using System.Threading.Tasks;
using GitLabKit.Runner.Core.Models;
using GitLabKit.Runner.Core.Repositories;

namespace GitLabKit.Runner.Core.Services;

public interface IGitLabGroupService
{
    Task<Group> GetGroup(int groupId);
}

public class GitLabGroupService : IGitLabGroupService
{
    private readonly IGitLabRepository _repository;

    public GitLabGroupService(IGitLabRepository repository)
    {
        _repository = repository;
    }

    public async Task<Group> GetGroup(int groupId) => await _repository.GetGroup(groupId);
}