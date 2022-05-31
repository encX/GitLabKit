using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GitLabApiClient;
using GitLabApiClient.Models.Runners.Requests;
using GitLabKit.Runner.Core.Configs;
using GitLabKit.Runner.Core.Exceptions;
using GitLabKit.Runner.Core.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Nelibur.ObjectMapper;
using Newtonsoft.Json;

namespace GitLabKit.Runner.Core.Repositories;

public interface IGitLabRepository
{
    Task<Group> GetGroup(int groupId);
    Task<IEnumerable<Models.Runner>> GetGroupRunners(int groupId);
    Task<Models.Runner> GetSingleRunner(int runnerId);
    Task<IEnumerable<Job>> GetRunnerCurrentJobs(int runnerId);
    Task<IEnumerable<Job>> GetRunnerJobs(int runnerId);
    Task InvalidateRunnerCache(int runnerId);
    Task<bool> SetRunnerActiveStatus(int runnerId, bool isActive);
    Task DeleteRunner(int runnerId);
}

public class GitLabRepository : IGitLabRepository
{
    private readonly ICache _cache;
    private readonly IGitLabClient _gitLabClient;
    private readonly HttpClient _gitLabHttpClient = new();

    private static string GroupRunnerListCacheKey(int id) => $"group_{id}_runnerlist";
    private static string GroupInfoCacheKey(int id) => $"group_{id}_info";
    private static string RunnerCacheKey(int id) => $"runner_{id}";

    public GitLabRepository(IOptions<Secrets> secretsOptions, IOptions<Connections> connectionOptions, ICache cache)
    {
        _cache = cache;
        _gitLabHttpClient.BaseAddress = new Uri($"{connectionOptions.Value.GitLabServer}/api/v4/");
        _gitLabHttpClient.DefaultRequestHeaders.Add("PRIVATE-TOKEN", secretsOptions.Value.GitLabToken);
        _gitLabClient = new GitLabClient(connectionOptions.Value.GitLabServer, secretsOptions.Value.GitLabToken);
    }

    public async Task<Group> GetGroup(int groupId)
    {
        var dataFunc = () => _gitLabClient.Groups.GetAsync(groupId);

        var groupInfo = await _cache.GetCached(GroupInfoCacheKey(groupId), dataFunc, TimeSpan.FromDays(1));

        return TinyMapper.Map<Group>(groupInfo);
    }

    public async Task<IEnumerable<Models.Runner>> GetGroupRunners(int groupId)
    {
        var dataFunc = () => GetPaginatedRequest<Models.Runner>(
            $"groups/{groupId}/runners?type=group_type",
            new Dictionary<string, string>());

        var runners = await _cache.GetCached(GroupRunnerListCacheKey(groupId), dataFunc, TimeSpan.FromMinutes(1));

        return runners;
    }

    public async Task<Models.Runner> GetSingleRunner(int runnerId)
    {
        var dataFunc = () => _gitLabClient.Runners.GetAsync(runnerId);
        var runner = await _cache.GetCached(RunnerCacheKey(runnerId), dataFunc, TimeSpan.FromMinutes(1));
        return TinyMapper.Map<Models.Runner>(runner);
    }

    public async Task<IEnumerable<Job>> GetRunnerCurrentJobs(int runnerId)
    {
        var dataFunc = () => GetDeserialized<List<Job>>(
            $"runners/{runnerId}/jobs",
            new Dictionary<string, string> {{"status", "running"}});

        var jobs = await _cache.GetCached($"runner_{runnerId}_currentjob", dataFunc, TimeSpan.FromMinutes(1));
        
        return jobs;
    }

    public async Task<IEnumerable<Job>> GetRunnerJobs(int runnerId)
    {
        var dataFunc = () => GetPaginatedRequest<Job>(
            $"runners/{runnerId}/jobs",
            new Dictionary<string, string>());

        var jobs = await _cache.GetCached($"runner_{runnerId}_alljob", dataFunc, TimeSpan.FromMinutes(1));

        return jobs;
    }
    
    public async Task InvalidateRunnerCache(int runnerId)
    {
        await _cache.Delete(RunnerCacheKey(runnerId));
    }

    public async Task<bool> SetRunnerActiveStatus(int runnerId, bool isActive)
    {
        var result = await _gitLabClient.Runners.UpdateAsync(runnerId, new UpdateRunnerRequest {Active = isActive});
        return result.Active;
    }

    public async Task DeleteRunner(int runnerId)
    {
        await _gitLabClient.Runners.DeleteAsync(runnerId);
    }

    private async Task<List<T>> GetPaginatedRequest<T>(string path, IDictionary<string, string> querystring)
    {
        List<T> currentPage;
        var all = new List<T>();
        var page = 1;

        do
        {
            querystring["page"] = page++.ToString();
            currentPage = await GetDeserialized<List<T>>(path, querystring);
            all.AddRange(currentPage);
        } while (currentPage.Count > 0);

        return all;
    }

    private async Task<T> GetDeserialized<T>(string path, IDictionary<string, string> querystring)
    {
        var url = QueryHelpers.AddQueryString(path, querystring);

        var res = await _gitLabHttpClient.GetAsync(url);

        if (res.StatusCode == HttpStatusCode.NotFound) throw new NotFoundException();

        if (!res.IsSuccessStatusCode)
        {
            throw new Exception($"GetDeserialized result: Status code {(int) res.StatusCode} : {res.ReasonPhrase} ({url})");
        }

        return JsonConvert.DeserializeObject<T>(await res.Content.ReadAsStringAsync());
    }
}