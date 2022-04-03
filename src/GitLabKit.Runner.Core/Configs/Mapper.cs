using GitLabApiClient.Models.Runners.Responses;
using GitLabKit.Runner.Core.Models;
using Nelibur.ObjectMapper;

namespace GitLabKit.Runner.Core.Configs;

public static class Mapper
{
    public static void Map()
    {
        TinyMapper.Bind<RunnerDetails, Models.Runner>(cfg =>
        {
            cfg.Bind(s => s.IpAddresses, t => t.IpAddress);
        });
        
        TinyMapper.Bind<GitLabApiClient.Models.Groups.Responses.Group, Group>();
    }
}