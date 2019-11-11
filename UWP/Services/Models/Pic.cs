using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class Pic
    {
        [JsonProperty("pid", NullValueHandling = NullValueHandling.Ignore)]
        public string Pid { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty("geo", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(PicGeoConverter))]
        public PicGeo Geo { get; set; }

        [JsonProperty("large", NullValueHandling = NullValueHandling.Ignore)]
        public Large Large { get; set; }
    }
}