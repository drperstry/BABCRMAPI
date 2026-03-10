using Microsoft.Extensions.Configuration;

namespace BabCrm.Core.Configurations
{
    public class CacheConfig : ICacheConfig
    {
        public const string ConfigKey = "Cache";
        public const string RedisDbName = "bab";

        public string RedisConnectionString
        {
            get;
            set;
        }

        public int CacheExpirationInMinutes
        {
            get;
            set;
        }

        public int ShortCacheExpirationInMinutes
        {
            get;
            set;
        }

        public bool DisableCaching
        {
            get;
            set;
        }

       

        public void LoadConfiguration(IConfiguration configuration)
        {
            var section = configuration.GetSection("Cache");

            section?.Bind(this);
        }
    }
}
