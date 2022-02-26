using BlazorCollaborativeApp.Shared.Services.Intefaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorCollaborativeApp.Shared.Services
{
    public class CachingService : ICachingService
    {
        private readonly IDistributedCache _cache;

        public CachingService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task DeleteAsync(string key)
        {
           await _cache.RemoveAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                var value = await _cache.GetStringAsync(key);
                if (value is not null)
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return JsonSerializer.Deserialize<T>(value);
#pragma warning restore CS8603 // Possible null reference return.
                }
                return default!;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return default!;
            }
        }

        public async Task<IReadOnlyList<T>> GetListAsync<T>(string key)
        {
            try
            {
                var value = await _cache.GetStringAsync(key);
                if (value is not null)
                {
#pragma warning disable CS8603 // Possible null reference return.
                    return JsonSerializer.Deserialize<IReadOnlyList<T>>(value);
#pragma warning restore CS8603 // Possible null reference return.
                }
                return default!;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return default!;
            }
        }

        public async Task SetAsync<T>(string key, T value)
        {
            try
            {
                var timeOut = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(48),
                    SlidingExpiration = TimeSpan.FromMinutes(60)
                };

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), timeOut);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        public async Task SetAsync<T>(string key, IList<T> value)
        {
            try
            {
                var timeOut = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(48),
                    SlidingExpiration = TimeSpan.FromMinutes(60)
                };

                await _cache.SetStringAsync(key, JsonSerializer.Serialize(value), timeOut);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }
    }
}
