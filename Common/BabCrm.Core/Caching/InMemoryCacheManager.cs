using BabCrm.Core.Configurations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabCrm.Core.Caching
{
    /// <summary>
    /// Class to manage caching using In Memory Cache.
    /// </summary>
    public class InMemoryCacheManager : ICacheManager
    {
        /// <summary>
        /// Distributed cache object.
        /// </summary>
        private readonly IMemoryCache cache;

        /// <summary>
        /// cache settings.
        /// </summary>
        private readonly ICacheConfig cacheConfig;

        public InMemoryCacheManager(IMemoryCache cache, ICacheConfig cacheConfig)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.cacheConfig = cacheConfig ?? throw new ArgumentNullException(nameof(cacheConfig));
        }

        /// <summary>
        /// Get item from cache or add it if don't exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of item in cache.</param>
        /// <param name="getItem">Delegeat to get the item.</param>
        /// <returns>The item to get.</returns>
        public T GetOrAddCachedObject<T>(string key, Func<T> getItem)
        {
            if (this.cacheConfig.DisableCaching)
            {
                return getItem();
            }

            var obj = this.cache.Get<T>(key);

            if (obj != null)
            {
                return obj;
            }
            else
            {
                var item = getItem();

                if (item == null)
                {
                    return default(T);
                }

                this.SetItemInCache<T>(key, item);

                return item;
            }
        }

        /// <summary>
        /// Get item from cache or add it async if don't exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of item in cache.</param>
        /// <param name="getItem">Delegeat to get the item.</param>
        /// <returns>The item to get.</returns>
        public async Task<T> GetOrAddCachedObjectAsync<T>(string key, Func<Task<T>> getItem, int itemExpiry = -1)
        {
            if (this.cacheConfig.DisableCaching)
            {
                return await getItem();
            }

            var obj = this.cache.Get<T>(key);

            if (obj != null)
            {
                return obj;
            }
            else
            {
                var item = await getItem();

                if (item == null)
                {
                    return default(T);
                }

                await SetItemInCacheAsync<T>(key, item, itemExpiry);

                return item;
            }
        }

        /// <summary>
        /// Refresh item in cache.
        /// </summary>
        /// <param name="key">key of the item in cache.</param>
        public void Refresh(string key)
        {
            //TODO
        }

        /// <summary>
        /// Refresh item in cache async.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns></returns>
        public Task RefreshAsync(string key)
        {
            //TODO
            return null;
        }

        /// <summary>
        /// Remove item from cache.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        public void RemoveItemsFromCache(params string[] keys)
        {
            if (this.cacheConfig.DisableCaching)
            {
                return;
            }

            foreach (var key in keys)
            {
                this.cache.Remove(key);
            }
        }

        /// <summary>
        /// Remove item from cache async.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns></returns>
        public async Task RemoveItemsFromCacheAsync(params string[] keys)
        {
            if (this.cacheConfig.DisableCaching)
            {
                return;
            }

            await Task.Factory.StartNew(() =>
            {
                foreach (var key in keys)
                {
                    this.cache.Remove(key);
                }
            });
        }

        /// <summary>
        /// Update an item in cache async.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Kye of the item in cache.</param>
        /// <param name="item">Item to update in cache.</param>D:\
        public async Task SetItemInCacheAsync<T>(string key, T item, int itemExpiry = -1)
        {
            if (this.cacheConfig.DisableCaching)
            {
                return;
            }

            if (item != null || ((item as IEnumerable)?.GetEnumerator().MoveNext() ?? false))
            {
                await Task.Factory.StartNew(() =>
                {
                    this.cache.Set<T>(key, item,
                        new MemoryCacheEntryOptions()
                        {
                            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(itemExpiry < 0 ? this.cacheConfig.CacheExpirationInMinutes : itemExpiry)
                        });
                });
            }
        }

        /// <summary>
        /// Update an item in cache.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Kye of the item in cache.</param>
        /// <param name="item">Item to update in cache.</param>
        public void SetItemInCache<T>(string key, T item, int itemExpiry = -1)
        {
            if (this.cacheConfig.DisableCaching)
            {
                return;
            }

            if (item != null || ((item as IEnumerable)?.GetEnumerator().MoveNext() ?? false))
            {
                this.cache.Set(key, item,
                new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(itemExpiry < 0 ? this.cacheConfig.CacheExpirationInMinutes : itemExpiry)
                });
            }
        }

        /// <summary>
        /// Get item from cache async if exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns>Item in cache.</returns>
        public async Task<T> GetItemFromCacheIfExistsAsync<T>(string key)
        {
            if (this.cacheConfig.DisableCaching)
            {
                return default(T);
            }

            return await Task.Factory.StartNew(() =>
            {
                var obj = this.cache.Get<T>(key);

                if (obj != null)
                {
                    return obj;
                }

                return default(T);
            });
        }

        /// <summary>
        /// Get item from cache if exists.
        /// </summary>
        /// <typeparam name="T">Type of the item.</typeparam>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns>Item in cache.</returns>
        public T GetItemFromCacheIfExists<T>(string key)
        {
            if (this.cacheConfig.DisableCaching)
            {
                return default(T);
            }

            var obj = this.cache.Get<T>(key);

            if (obj != null)
            {
                return obj;
            }

            return default(T);
        }

        public IEnumerable GetKeys()
        {
            if (this.cacheConfig.DisableCaching)
            {
                return Enumerable.Empty<string>();
            }

            var obj = this.cache.GetKeys();

            if (obj != null)
            {
                return obj;
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Get item from cache async if exists.
        /// </summary>
        /// <param name="key">Key of the item in cache.</param>
        /// <returns>Item in cache.</returns>
        public async Task<object> GetItem(string key)
        {
            if (this.cacheConfig.DisableCaching)
            {
                return null;
            }

            return await Task.Factory.StartNew(() => this.cache.Get(key));
        }
    }
}
