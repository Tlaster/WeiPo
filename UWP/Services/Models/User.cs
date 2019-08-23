using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class User
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("screen_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ScreenName { get; set; }

        [JsonProperty("profile_image_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ProfileImageUrl { get; set; }

        [JsonProperty("avatar_large", NullValueHandling = NullValueHandling.Ignore)]
        public Uri AvatarLarge { get; set; }

        [JsonProperty("verified", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Verified { get; set; }

        [JsonProperty("verified_type", NullValueHandling = NullValueHandling.Ignore)]
        public long? VerifiedType { get; set; }

        [JsonProperty("level", NullValueHandling = NullValueHandling.Ignore)]
        public long? Level { get; set; }

        [JsonProperty("name")]
        public object Name { get; set; }

        [JsonProperty("followers_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? FollowersCount { get; set; }

        [JsonProperty("mbtype")]
        public object Mbtype { get; set; }

        [JsonProperty("mbrank")]
        public object Mbrank { get; set; }

        [JsonProperty("remark")]
        public object Remark { get; set; }

        [JsonProperty("friends_count", NullValueHandling = NullValueHandling.Ignore)]
        public long? FriendsCount { get; set; }

        [JsonProperty("icons", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Icons { get; set; }

        [JsonProperty("verified_type_ext", NullValueHandling = NullValueHandling.Ignore)]
        public long? VerifiedTypeExt { get; set; }

        [JsonProperty("following")]
        public object Following { get; set; }
    }
}