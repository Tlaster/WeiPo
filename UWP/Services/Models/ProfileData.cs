using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class ProfileData
    {
        [JsonProperty("avatar_guide", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AvatarGuide { get; set; }

        [JsonProperty("userInfo", NullValueHandling = NullValueHandling.Ignore)]
        public UserInfo UserInfo { get; set; }

        [JsonProperty("fans_scheme", NullValueHandling = NullValueHandling.Ignore)]
        public Uri FansScheme { get; set; }

        [JsonProperty("follow_scheme", NullValueHandling = NullValueHandling.Ignore)]
        public Uri FollowScheme { get; set; }

        [JsonProperty("tabsInfo", NullValueHandling = NullValueHandling.Ignore)]
        public TabsInfo TabsInfo { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }

        [JsonProperty("showAppTips", NullValueHandling = NullValueHandling.Ignore)]
        public long? ShowAppTips { get; set; }
    }

    public partial class TabsInfo
    {
        [JsonProperty("selectedTab", NullValueHandling = NullValueHandling.Ignore)]
        public long SelectedTab { get; set; }

        [JsonProperty("tabs", NullValueHandling = NullValueHandling.Ignore)]
        public Tab[] Tabs { get; set; }
    }

    public partial class Tab
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

    public partial class FilterGroupInfo
    {
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("icon", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Icon { get; set; }

        [JsonProperty("icon_name", NullValueHandling = NullValueHandling.Ignore)]
        public string IconName { get; set; }

        [JsonProperty("icon_scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string IconScheme { get; set; }
    }

    public partial class UserInfo : UserModel
    {
        [JsonProperty("toolbar_menus", NullValueHandling = NullValueHandling.Ignore)]
        public ToolbarMenu[] ToolbarMenus { get; set; }
    }

    public partial class ToolbarMenu
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("sub_type", NullValueHandling = NullValueHandling.Ignore)]
        public long? SubType { get; set; }

        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public ToolbarMenuParams Params { get; set; }

        [JsonProperty("actionlog", NullValueHandling = NullValueHandling.Ignore)]
        public Actionlog Actionlog { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Scheme { get; set; }
    }


    public partial class ToolbarMenuParams
    {
        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public long? Uid { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }

        [JsonProperty("menu_list", NullValueHandling = NullValueHandling.Ignore)]
        public MenuList[] MenuList { get; set; }
    }

    public partial class MenuList
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public MenuListParams Params { get; set; }

        [JsonProperty("actionlog", NullValueHandling = NullValueHandling.Ignore)]
        public Actionlog Actionlog { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Scheme { get; set; }
    }
    
    public partial class MenuListParams
    {
        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }
    }
}
