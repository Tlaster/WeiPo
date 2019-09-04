using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class Urls
    {
        [JsonProperty("mp4_720p_mp4", NullValueHandling = NullValueHandling.Ignore)]
        public string Mp4720PMp4 { get; set; }

        [JsonProperty("mp4_hd_mp4", NullValueHandling = NullValueHandling.Ignore)]
        public string Mp4HdMp4 { get; set; }

        [JsonProperty("mp4_ld_mp4", NullValueHandling = NullValueHandling.Ignore)]
        public string Mp4LdMp4 { get; set; }
    }
}