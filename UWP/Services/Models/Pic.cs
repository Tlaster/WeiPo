using System;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class Pic
    {
        [JsonProperty("pid", NullValueHandling = NullValueHandling.Ignore)]
        public string Pid { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty("geo", NullValueHandling = NullValueHandling.Ignore)]
        public PicGeo Geo { get; set; }

        [JsonProperty("large", NullValueHandling = NullValueHandling.Ignore)]
        public Large Large { get; set; }
    }
}