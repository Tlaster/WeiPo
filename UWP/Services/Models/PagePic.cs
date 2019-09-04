using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class PagePic
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long Height { get; set; }
    }
}