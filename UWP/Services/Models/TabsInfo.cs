using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class TabsInfo
    {
        [JsonProperty("selectedTab", NullValueHandling = NullValueHandling.Ignore)]
        public int SelectedTab { get; set; }

        [JsonProperty("tabs", NullValueHandling = NullValueHandling.Ignore)]
        public Tab[] Tabs { get; set; }
    }
}