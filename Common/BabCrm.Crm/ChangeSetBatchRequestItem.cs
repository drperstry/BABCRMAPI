using Newtonsoft.Json.Linq;

namespace BabCrm.Crm
{
    public sealed class ChangeSetBatchRequestItem
    {
        public string EntityPluralName
        {
            get;
            set;
        }

        public JObject PostObject
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{EntityPluralName} => {PostObject.ToString()}";
        }
    }
}
