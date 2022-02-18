using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorCollaborativeApp.Shared.Services.Intefaces
{
    public interface ICachingService
    {
        /// <summary>
        /// Get from data to cache
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="key">Unique data identifier.</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// set specific data to cache.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="key">Unique data identifier.</param>
        /// <param name="value">Data to save.</param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value);
    }
}
