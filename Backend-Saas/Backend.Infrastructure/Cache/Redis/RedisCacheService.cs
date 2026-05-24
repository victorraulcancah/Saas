namespace Backend.Infrastructure.Cache.Redis;

using StackExchange.Redis;
using System.Text.Json;

public class RedisCacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.HasValue ? JsonSerializer.Deserialize<T>((string)value!) : default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var serialized = JsonSerializer.Serialize(value);
        if (expiry.HasValue)
            await _database.StringSetAsync(key, serialized, expiry.Value);
        else
            await _database.StringSetAsync(key, serialized);
    }

    public async Task RemoveAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }
}
