#nullable enable
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf.Collections;

namespace ON.Mercury.Service.Caching;

public interface ICachingService
{
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class;

    Task<T> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default)  where T : class;

    Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        where T : class;

    Task AddOrSetAsync<T>(string key, IEnumerable<T> value, CancellationToken cancellationToken = default) where T : IMessage<T>;

    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
}