using System;
using System.Collections;
using System.Threading.Tasks;

namespace BabCrm.Core.Caching
{
    /// <summary>
    /// Contract to manage caching.
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Get item from cache or add it async if don't exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of item in cache.</param>
        /// <param name="getItem">Delegeat to get the item.</param>
        /// <returns>The item to get.</returns>
        Task<T> GetOrAddCachedObjectAsync<T>(string key, Func<Task<T>> getItem, int itemExpiry = -1);

        /// <summary>
        /// Get item from cache or add it if don't exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of item in cache.</param>
        /// <param name="getItem">Delegeat to get the item.</param>
        /// <returns>The item to get.</returns>
        T GetOrAddCachedObject<T>(string key, Func<T> getItem);

        /// <summary>
        /// Remove item from cache.
        /// </summary>
        /// <param name="keys">Key of the item in cache.</param>
        void RemoveItemsFromCache(params string[] keys);

        /// <summary>
        /// Remove item from cache async.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns></returns>
        Task RemoveItemsFromCacheAsync(params string[] keys);

        /// <summary>
        /// Refresh item in cache.
        /// </summary>
        /// <param name="key">key of the item in cache.</param>
        void Refresh(string key);

        /// <summary>
        /// Refresh item in cache async.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns></returns>
        Task RefreshAsync(string key);

        /// <summary>
        /// Set an item in cache.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Kye of the item in cache.</param>
        /// <param name="item">Item to update in cache.</param>
        void SetItemInCache<T>(string key, T item, int itemExpiry = -1);

        /// <summary>
        /// Set an item in cache async.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of the item in cache.</param>
        /// <param name="item">Item to update in cache.</param>
        Task SetItemInCacheAsync<T>(string key, T item, int itemExpiry = -1);

        /// <summary>
        /// Get item from cache async if exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns>Item in cache.</returns>
        Task<T> GetItemFromCacheIfExistsAsync<T>(string key);

        IEnumerable GetKeys();

        /// <summary>
        /// Get item from cache if exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns>Item in cache.</returns>
        T GetItemFromCacheIfExists<T>(string key);

        /// <summary>
        /// Get item from cache async if exists.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns>Item in cache.</returns>
        Task<object> GetItem(string key);
    }
}
