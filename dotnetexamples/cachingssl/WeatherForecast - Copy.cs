using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;

namespace cachingssl
{
    public interface IRedisDistributedCache : IDistributedCache
    {

    }
    public class RedisDistributedCache : RedisCache, IRedisDistributedCache
    {
        public RedisDistributedCache(IOptions<RedisCacheOptions> optionsAccessor) :
            base(optionsAccessor)
        {

        }

    }
}