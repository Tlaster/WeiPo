using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class LargeGeo
    {
        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Height { get; set; }

        [JsonProperty("croped", NullValueHandling = NullValueHandling.Ignore)]
        public bool Croped { get; set; }
    }
}