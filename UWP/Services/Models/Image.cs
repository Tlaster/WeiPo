using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class Image
    {
        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }
}