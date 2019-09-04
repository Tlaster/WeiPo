using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class Visible
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public long Type { get; set; }

        [JsonProperty("list_id", NullValueHandling = NullValueHandling.Ignore)]
        public long ListId { get; set; }
    }
}