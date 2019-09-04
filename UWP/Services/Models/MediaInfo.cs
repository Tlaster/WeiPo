using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class MediaInfo
    {
        [JsonProperty("stream_url", NullValueHandling = NullValueHandling.Ignore)]
        public string StreamUrl { get; set; }

        [JsonProperty("stream_url_hd", NullValueHandling = NullValueHandling.Ignore)]
        public string StreamUrlHd { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public double Duration { get; set; }

        [JsonProperty("mp4_720p_mp4", NullValueHandling = NullValueHandling.Ignore)]
        public string Mp4720pMp4 { get; set; }
    }
}