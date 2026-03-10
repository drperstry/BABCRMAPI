using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabCrm.Service.Crm.Models
{
    internal class LabelInfo
    {
        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("LanguageCode")]
        public int LanguageCode { get; set; }

        [JsonProperty("IsManaged")]
        public bool IsManaged { get; set; }

        [JsonProperty("MetadataId")]
        public string MetadataId { get; set; }

        [JsonProperty("HasChanged")]
        public bool? HasChanged { get; set; }
    }
}
