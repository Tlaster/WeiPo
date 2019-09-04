using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class Tab
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long Id { get; set; }

        [JsonProperty("tabKey", NullValueHandling = NullValueHandling.Ignore)]
        public string TabKey { get; set; }

        [JsonProperty("must_show", NullValueHandling = NullValueHandling.Ignore)]
        public long MustShow { get; set; }

        [JsonProperty("hidden", NullValueHandling = NullValueHandling.Ignore)]
        public long Hidden { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("tab_type", NullValueHandling = NullValueHandling.Ignore)]
        public string TabType { get; set; }

        [JsonProperty("containerid", NullValueHandling = NullValueHandling.Ignore)]
        public string Containerid { get; set; }

        [JsonProperty("apipath", NullValueHandling = NullValueHandling.Ignore)]
        public string Apipath { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }

        [JsonProperty("filter_group", NullValueHandling = NullValueHandling.Ignore)]
        public FilterGroup[] FilterGroup { get; set; }

        [JsonProperty("filter_group_info", NullValueHandling = NullValueHandling.Ignore)]
        public FilterGroupInfo FilterGroupInfo { get; set; }
    }
}