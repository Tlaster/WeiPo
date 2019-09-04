using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class UserInfo : UserModel
    {
        [JsonProperty("toolbar_menus", NullValueHandling = NullValueHandling.Ignore)]
        public ToolbarMenu[] ToolbarMenus { get; set; }
    }
}