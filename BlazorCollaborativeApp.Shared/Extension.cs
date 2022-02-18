using BlazorCollaborativeApp.Shared.Services;
using BlazorCollaborativeApp.Shared.Services.Intefaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"localhost,abortConnect=false,connectTimeout=30000,responseTimeout=30000";
                options.InstanceName = "blazorCollaborativeAppCache"; 
            });

            services.AddTransient<ICachingService, CachingService>();
        }
    }
}
