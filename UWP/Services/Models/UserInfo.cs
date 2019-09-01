using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class UserInfo : UserModel
    {
        [JsonProperty("toolbar_menus", NullValueHandling = NullValueHandling.Ignore)]
        public ToolbarMenu[] ToolbarMenus { get; set; }
    }
}