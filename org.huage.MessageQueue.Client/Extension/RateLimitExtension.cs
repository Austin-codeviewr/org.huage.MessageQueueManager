using AspNetCoreRateLimit;
using Castle.Windsor.Installer;
using Microsoft.Extensions.Caching.Distributed;
using org.huage.MessageQueue.Client.service;
using StackExchange.Redis;


namespace org.huage.MessageQueue.Client.Extension;

public static class RateLimitExtension
{
    public static void RegisterRateLimit(this IServiceCollection services,IConfiguration configuration)
    {
        //需要从加载配置文件appsettings.json
        services.AddOptions();
        //需要存储速率限制计算器和ip规则
        //services.AddMemoryCache();
        
        //从appsettings.json中加载常规配置，IpRateLimiting与配置文件中节点对应
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

        //从appsettings.json中加载Ip规则
        services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
       
        //注入计数器和规则存储
        services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitCounterStore,DistributedCacheRateLimitCounterStore>();

        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //配置（解析器、计数器密钥生成器）
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new ConfigurationOptions
            {
                //silently retry in the background if the Redis connection is temporarily down
                AbortOnConnectFail = false,
                EndPoints = { "127.0.0.1", "6379" },
                ConnectRetry = 5,
                Ssl = false,
                ConnectTimeout = 5000,
                SyncTimeout = 5000,
                DefaultDatabase = 0
            };
            options.InstanceName = "Austin-RateLimit";
        });
    }
    
    
}