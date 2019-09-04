using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class PicGeo
    {
        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long Height { get; set; }

        [JsonProperty("croped", NullValueHandling = NullValueHandling.Ignore)]
        public bool Croped { get; set; }
    }
}