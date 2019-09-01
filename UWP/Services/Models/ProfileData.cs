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
        public string FansScheme { get; set; }

        [JsonProperty("follow_scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string FollowScheme { get; set; }

        [JsonProperty("tabsInfo", NullValueHandling = NullValueHandling.Ignore)]
        public TabsInfo TabsInfo { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }

        [JsonProperty("showAppTips", NullValueHandling = NullValueHandling.Ignore)]
        public long? ShowAppTips { get; set; }
    }
}
