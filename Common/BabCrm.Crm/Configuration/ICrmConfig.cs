
namespace BabCrm.Crm.Configuration
{
    using BabCrm.Core.Configurations;

    public interface ICrmConfig : IBabCrmConfig
    {
        bool IsIfd
        {
            get;
        }

        string ServiceUrl
        {
            get;
        }

        string BaseAddress
        {
            get;
        }

        string Domain
        {
            get;
        }

        string Name
        {
            get;
        }

        string Code
        {
            get;
        }

        string FakeUserProfileId
        {
            get;
        }

        string ClientExternal
        {
            get;
        }

        string ClientInternal
        {
            get;
        }

        string AdfsUrl
        {
            get;
        }
    }
}
