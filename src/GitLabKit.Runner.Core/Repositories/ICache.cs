using System;
using System.Threading.Tasks;

namespace GitLabKit.Runner.Core.Repositories;

public interface ICache
{
    Task<T> GetCached<T>(string key, Func<Task<T>> dataFunc, TimeSpan ttl);
    Task Delete(string key);
}