
namespace BabCrm.Crm
{
    using Newtonsoft.Json.Linq;

    public class BatchResponse
    {
        public string EntityPluralName
        {
            get;
            set;
        }

        public JArray Data
        {
            get;
            set;
        }
    }
}
