namespace BabCrm.Core.Configurations
{
    public interface ICacheConfig : IBabCrmConfig
    {
       

        /// <summary>
        /// Gets cache expiration in minutes.
        /// </summary>
        int CacheExpirationInMinutes
        {
            get;
        }

        /// <summary>
        /// Gets short cache expiration in minutes.
        /// </summary>
        int ShortCacheExpirationInMinutes
        {
            get;
        }

        /// <summary>
        /// Gets a boolean indicate if disable or enable caching.
        /// </summary>
        bool DisableCaching
        {
            get;
        }

        
    }
}
