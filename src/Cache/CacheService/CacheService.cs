using CacheService.Abstractions;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace CacheService
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;
        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _database = connectionMultiplexer.GetDatabase();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var redisValue = await _database.StringGetAsync(key);
            if (redisValue.IsNullOrEmpty)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(redisValue);
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var serializedValue = JsonConvert.SerializeObject(value);
            return await _database.StringSetAsync(key, serializedValue, expiry);
        }
    }
}
