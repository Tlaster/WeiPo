using System;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class MediaInfo
    {
        [JsonProperty("stream_url", NullValueHandling = NullValueHandling.Ignore)]
        public string StreamUrl { get; set; }

        [JsonProperty("stream_url_hd", NullValueHandling = NullValueHandling.Ignore)]
        public string StreamUrlHd { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public double Duration { get; set; }
    }
}