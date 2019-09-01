using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class ToolbarMenuParams
    {
        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public long? Uid { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }

        [JsonProperty("menu_list", NullValueHandling = NullValueHandling.Ignore)]
        public MenuList[] MenuList { get; set; }
    }
}