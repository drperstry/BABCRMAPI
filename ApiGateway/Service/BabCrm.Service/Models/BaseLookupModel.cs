using BabCrm.ObjectModel;

namespace BabCrm.Service.Models
{
    public class BaseLookupModel
    {
        public string Id { get; set; }

        public LocalizedValue<string> Name { get; set; }

        public string Code { get; set; }
    }

    public class LookupModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }

    public class PropertyModel
    {

        public string Name { get; set; }

        public string Type { get; set; }

    }
}
