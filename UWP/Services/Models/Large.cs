using System;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class Large
    {
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("geo", NullValueHandling = NullValueHandling.Ignore)]
        public LargeGeo Geo { get; set; }
    }
}