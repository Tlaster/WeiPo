using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class FilterGroup
    {
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("containerid", NullValueHandling = NullValueHandling.Ignore)]
        public string Containerid { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }
    }
}