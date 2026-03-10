using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabCrm.Crm.Configuration
{
    public class CrmConfig : ICrmConfig
    {
        public bool IsIfd
        {
            get;
            set;
        }

        public string ServiceUrl
        {
            get;
            set;
        }

        public string BaseAddress
        {
            get;
            set;
        }

        public string Domain
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string FakeUserProfileId
        {
            get;
            set;
        }

        public string ClientExternal
        {
            get;
            set;
        }

        public string ClientInternal
        {
            get;
            set;
        }

        public string AdfsUrl
        {
            get;
            set;
        }

        public void LoadConfiguration(IConfiguration configuration)
        {
            var section = configuration.GetSection("Crm");

            section.Bind(this);
        }
    }
}
