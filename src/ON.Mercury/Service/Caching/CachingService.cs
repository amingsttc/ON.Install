#nullable enable
using Google.Protobuf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.Collections;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ON.Mercury.Service.Caching;

public class CachingService : ICachingService
    {
        // TODO: Figure out cache invalidation
        private static readonly ConcurrentDictionary<string, bool> CacheKeys = new ConcurrentDictionary<string, bool>();
        private readonly IDistributedCache _distributedCache;
        
        public CachingService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
        {
            var cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

            if (cachedValue is null) return null;

            var value = JsonConvert.DeserializeObject<T>(cachedValue);
            return value;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
        {
            var cachedValue = await GetAsync<T>(key, cancellationToken);
            if (cachedValue is null)
            {
                cachedValue = await factory();
                await SetAsync<T>(key, cachedValue, cancellationToken);
                return cachedValue;
            }

            return cachedValue;
        }

        public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
        {
            var cacheValue = JsonConvert.SerializeObject(value);
            await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);
            CacheKeys.TryAdd(key, false);
        }

        public async Task AddOrSetAsync<T>(string key, IEnumerable<T> value, CancellationToken cancellationToken = default) where T : IMessage<T>
        {
            var cached = await _distributedCache.GetStringAsync(key, cancellationToken);
            if (!string.IsNullOrEmpty(cached))
            {
                var list = JsonConvert.DeserializeObject<RepeatedField<T>>(cached);
                list.Add(value);
                await SetAsync(key, list, cancellationToken);
                return;
            }

            await SetAsync(key, value, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
            CacheKeys.TryRemove(key, out bool _);
        }

        public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
        {
            IEnumerable<Task> tasks = CacheKeys.Keys
                .Where(k => k.StartsWith(prefixKey))
                .Select(k => RemoveAsync(k, cancellationToken));
            
            await Task.WhenAll(tasks);
        }
    }