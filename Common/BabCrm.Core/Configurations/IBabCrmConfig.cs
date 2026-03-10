using Microsoft.Extensions.Configuration;

namespace BabCrm.Core.Configurations
{
    public interface IBabCrmConfig
    {
        /// <summary>
        /// Load conifguration from configuration section.
        /// </summary>
        /// <param name="configuration"></param>
        void LoadConfiguration(IConfiguration configuration);
    }
}
