using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class Params
    {
        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public long? Uid { get; set; }

        [JsonProperty("need_follow", NullValueHandling = NullValueHandling.Ignore)]
        public long? NeedFollow { get; set; }

        [JsonProperty("trend_ext", NullValueHandling = NullValueHandling.Ignore)]
        public string TrendExt { get; set; }

        [JsonProperty("trend_type", NullValueHandling = NullValueHandling.Ignore)]
        public long? TrendType { get; set; }

        [JsonProperty("itemid", NullValueHandling = NullValueHandling.Ignore)]
        public long? Itemid { get; set; }

        [JsonProperty("allow_replenish", NullValueHandling = NullValueHandling.Ignore)]
        public long? AllowReplenish { get; set; }

        [JsonProperty("api_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ApiType { get; set; }
    }
}