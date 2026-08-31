namespace MedOps.Infrastructure.Services;

using MedOps.Infrastructure.Interfaces;
using StackExchange.Redis;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _db;

    public RedisCacheService(string connectionString)
    {
        var multiplexer = ConnectionMultiplexer.Connect(connectionString);
        _db = multiplexer.GetDatabase();
    }

    public async Task SetAsync(string key, string value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        await _db.StringSetAsync(key, value, expiry, flags: CommandFlags.FireAndForget);
    }

    public async Task<string?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _db.StringGetAsync(key);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task<bool> KeyExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        return await _db.KeyExistsAsync(key);
    }
}