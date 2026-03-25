using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Odyssey.UI.Core.Models
{
    public class HtmxSignalRMessage
    {
        [JsonProperty("HEADERS")]
        public Dictionary<string, object> Headers { get; set; } = [];

        public required string Type { get; set; }

        [JsonExtensionData]
        public required JObject Body { get; set; }
    }
}
