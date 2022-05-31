using GitLabKit.Runner.Core.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace GitLabKit.Runner.Web.Startup;

public static class CacheConfigurator
{
    public static void AddCache(this IServiceCollection service, IConfiguration connectionsConfig, IConfiguration secretsConfig)
    {
        var redisServerUrl = connectionsConfig.GetValue<string>("RedisServer");

        if (!string.IsNullOrEmpty(redisServerUrl))
        {
            service.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisServerUrl,
                    options =>
                    {
                        options.AbortOnConnectFail = false;
                        options.ConnectTimeout = 1000;

                        var user = secretsConfig.GetValue<string>("RedisUser");
                        var password = secretsConfig.GetValue<string>("RedisPassword");

                        if (!string.IsNullOrEmpty(user)) options.User = user;
                        if (!string.IsNullOrEmpty(password)) options.Password = password;
                    }));

            service.AddSingleton<ICache, RedisCache>();
        }
        else
        {
            service.AddSingleton<ICache, NoCache>();
        }
    }
}