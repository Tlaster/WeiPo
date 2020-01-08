using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class OriginalImage
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }
}