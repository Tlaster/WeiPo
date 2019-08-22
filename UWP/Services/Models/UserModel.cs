using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class UserModel
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long Id { get; set; }

        [JsonProperty("screen_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ScreenName { get; set; }

        [JsonProperty("profile_image_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ProfileImageUrl { get; set; }

        [JsonProperty("profile_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ProfileUrl { get; set; }

        [JsonProperty("statuses_count", NullValueHandling = NullValueHandling.Ignore)]
        public long StatusesCount { get; set; }

        [JsonProperty("verified", NullValueHandling = NullValueHandling.Ignore)]
        public bool Verified { get; set; }

        [JsonProperty("verified_type", NullValueHandling = NullValueHandling.Ignore)]
        public long VerifiedType { get; set; }

        [JsonProperty("verified_type_ext", NullValueHandling = NullValueHandling.Ignore)]
        public long VerifiedTypeExt { get; set; }

        [JsonProperty("verified_reason", NullValueHandling = NullValueHandling.Ignore)]
        public string VerifiedReason { get; set; }

        [JsonProperty("close_blue_v", NullValueHandling = NullValueHandling.Ignore)]
        public bool CloseBlueV { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        public string Gender { get; set; }

        [JsonProperty("mbtype", NullValueHandling = NullValueHandling.Ignore)]
        public long Mbtype { get; set; }

        [JsonProperty("urank", NullValueHandling = NullValueHandling.Ignore)]
        public long Urank { get; set; }

        [JsonProperty("mbrank", NullValueHandling = NullValueHandling.Ignore)]
        public long Mbrank { get; set; }

        [JsonProperty("follow_me", NullValueHandling = NullValueHandling.Ignore)]
        public bool FollowMe { get; set; }

        [JsonProperty("following", NullValueHandling = NullValueHandling.Ignore)]
        public bool Following { get; set; }

        [JsonProperty("followers_count", NullValueHandling = NullValueHandling.Ignore)]
        public long FollowersCount { get; set; }

        [JsonProperty("follow_count", NullValueHandling = NullValueHandling.Ignore)]
        public long FollowCount { get; set; }

        [JsonProperty("cover_image_phone", NullValueHandling = NullValueHandling.Ignore)]
        public string CoverImagePhone { get; set; }

        [JsonProperty("avatar_hd", NullValueHandling = NullValueHandling.Ignore)]
        public string AvatarHd { get; set; }

        [JsonProperty("like", NullValueHandling = NullValueHandling.Ignore)]
        public bool Like { get; set; }

        [JsonProperty("like_me", NullValueHandling = NullValueHandling.Ignore)]
        public bool LikeMe { get; set; }

        [JsonProperty("badge", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, long> Badge { get; set; }
    }
}