using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class Unreadmsg
    {
        [JsonProperty("1", NullValueHandling = NullValueHandling.Ignore)]
        public long? The1 { get; set; }

        [JsonProperty("3", NullValueHandling = NullValueHandling.Ignore)]
        public long? The3 { get; set; }

        [JsonProperty("4", NullValueHandling = NullValueHandling.Ignore)]
        public long? The4 { get; set; }

        [JsonProperty("8", NullValueHandling = NullValueHandling.Ignore)]
        public long? The8 { get; set; }
    }
}