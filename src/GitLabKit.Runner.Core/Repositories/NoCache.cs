using System;
using System.Threading.Tasks;

namespace GitLabKit.Runner.Core.Repositories;

public class NoCache : ICache
{
    public async Task<T> GetCached<T>(string key, Func<Task<T>> dataFunc, TimeSpan ttl)
    {
        return await dataFunc.Invoke();
    }

    public Task Delete(string key)
    {
        return Task.CompletedTask;
    }
}