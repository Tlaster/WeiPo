using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class VideoDetails
    {
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public long Size { get; set; }

        [JsonProperty("bitrate", NullValueHandling = NullValueHandling.Ignore)]
        public long Bitrate { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        [JsonProperty("prefetch_size", NullValueHandling = NullValueHandling.Ignore)]
        public long PrefetchSize { get; set; }
    }
}