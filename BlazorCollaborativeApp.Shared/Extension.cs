using BlazorCollaborativeApp.Shared.Services;
using BlazorCollaborativeApp.Shared.Services.Intefaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCollaborativeApp.Shared
{
    public static class Extension
    {

        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConfig = ConfigurationOptions.Parse($"{configuration["Redis:Host"]}:{configuration["Redis:Port"]}");
            redisConfig.AbortOnConnectFail = false;
            redisConfig.AllowAdmin = true;
            services.AddStackExchangeRedisCache(options =>
            {
                options.ConfigurationOptions = redisConfig;
                options.InstanceName = "blazorCollaborativeAppCache"; 
            });

            services.AddTransient<ICachingService, CachingService>();
        }
    }
}
