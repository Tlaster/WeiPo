using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class Large
    {
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("geo", NullValueHandling = NullValueHandling.Ignore)]
        public LargeGeo Geo { get; set; }
    }
}