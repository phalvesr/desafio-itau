using System.Text.Json;
using Identity.Server.Domain.Gateways;
using StackExchange.Redis;

namespace Identity.Server.Infrastructure.DataProviders.Repositories;

public class CacheRepository<T> : ICacheRepository<T>
{
    private readonly IConnectionMultiplexer connectionMultiplexer;

    public CacheRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        this.connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<T?> GetEntryOrDefaultAsync(string key)
    {
        ValidateKeyAndThrow(key);

        var db = connectionMultiplexer.GetDatabase();

        var entryAsJson = await db.StringGetAsync(key);

        if (entryAsJson == RedisValue.Null)
        {
            return default;
        }

        var entry = JsonSerializer.Deserialize<T>(entryAsJson);

        return entry;
    }

    public async Task SetEntryAsync(string key, T unserializedObject, TimeSpan? expiry = null)
    {
        ValidateKeyAndThrow(key);
        ValidateValueAndThrow(unserializedObject);

        var serializedObject = JsonSerializer.Serialize(unserializedObject);

        var db = connectionMultiplexer.GetDatabase();

        await db.StringSetAsync(key, serializedObject, expiry);
    }

    private void ValidateKeyAndThrow(string key)
    {
        if (!IsValidKey(key))
        {
            throw new ArgumentException("Provided value is not a valid key.", nameof(key));
        }
    }

    private bool IsValidKey(string key)
    {
        return !(string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key));
    }

    private void ValidateValueAndThrow(T unserializedObject)
    {
        if (unserializedObject is null)
        {
            throw new ArgumentException("Provided object must not be null.", nameof(unserializedObject));
        }
    }
}
