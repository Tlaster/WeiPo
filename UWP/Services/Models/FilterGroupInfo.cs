using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class FilterGroupInfo
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public string Icon { get; set; }

        [JsonProperty("icon_name", NullValueHandling = NullValueHandling.Ignore)]
        public string IconName { get; set; }

        [JsonProperty("icon_scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string IconScheme { get; set; }
    }
}