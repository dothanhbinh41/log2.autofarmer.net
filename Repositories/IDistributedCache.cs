using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson.Serialization.Serializers;

namespace LogJson.AutoFarmer.Repositories
{
    public interface IAutoFarmerDistributeCache
    {
        Task<T?> GetAsync<T>(string key, CancellationToken token = default(CancellationToken));
        Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken));
        Task RefreshAsync(string key, CancellationToken token = default(CancellationToken));
        Task RemoveAsync(string key, CancellationToken token = default(CancellationToken));
        Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> func, DistributedCacheEntryOptions options, CancellationToken token = default(CancellationToken));
    }

    public class AutoFarmerDistributeCache : IAutoFarmerDistributeCache
    {
        private readonly IDistributedCache cache;
        private readonly IObjectSerializer objectSerializer;

        public AutoFarmerDistributeCache(IDistributedCache cache, IObjectSerializer objectSerializer)
        {
            this.cache = cache;
            this.objectSerializer = objectSerializer;
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken token = default)
        {
            var value = await cache.GetAsync(key, token);
            if (value == null)
            {
                return default(T?);
            }

            return objectSerializer.Deserialize<T>(value);
        }

        public async Task<T?> GetOrAddAsync<T>(string key, Func<Task<T>> func, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var objFromCache = await GetAsync<T>(key, token);
            if (objFromCache != null)
            {
                return objFromCache;
            }


            var objFromInit = await func.Invoke();
            if (objFromInit == null)
            {
                return default(T?);
            }

            await SetAsync(key, objFromInit, options, token);
            return objFromInit;
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            return cache.RefreshAsync(key, token);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            return cache.RemoveAsync(key, token);
        }

        public Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            var obj = objectSerializer.Serialize(value);
            return cache.SetAsync(key, obj, options, token);
        }
    }
}
