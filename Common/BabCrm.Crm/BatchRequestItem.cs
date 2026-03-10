
namespace BabCrm.Crm
{
    public sealed class BatchRequestItem
    {
        public string EntityPluralName
        {
            get;
            set;
        }

        public string FetchXml
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"{EntityPluralName} => {FetchXml}";
        }
    }
}
