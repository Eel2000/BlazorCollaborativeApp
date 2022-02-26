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
        /// get data list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<IReadOnlyList<T>> GetListAsync<T>(string key);

        /// <summary>
        /// set specific data to cache.
        /// </summary>
        /// <typeparam name="T">Data type.</typeparam>
        /// <param name="key">Unique data identifier.</param>
        /// <param name="value">Data to save.</param>
        /// <returns></returns>
        Task SetAsync<T>(string key, T value);
        Task SetAsync<T>(string key, IList<T> value);

        /// <summary>
        /// Remove the specifique sheets
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task DeleteAsync(string key);
    }
}
