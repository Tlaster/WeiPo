using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class UnreadModel
    {
        [JsonProperty("cmt", NullValueHandling = NullValueHandling.Ignore)]
        public long Cmt { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public long Status { get; set; }

        [JsonProperty("follower", NullValueHandling = NullValueHandling.Ignore)]
        public long Follower { get; set; }

        [JsonProperty("dm", NullValueHandling = NullValueHandling.Ignore)]
        public long Dm { get; set; }

        [JsonProperty("mention_cmt", NullValueHandling = NullValueHandling.Ignore)]
        public long MentionCmt { get; set; }

        [JsonProperty("mention_status", NullValueHandling = NullValueHandling.Ignore)]
        public long MentionStatus { get; set; }

        [JsonProperty("attitude", NullValueHandling = NullValueHandling.Ignore)]
        public long Attitude { get; set; }

        [JsonProperty("unreadmblog", NullValueHandling = NullValueHandling.Ignore)]
        public long Unreadmblog { get; set; }

        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public string Uid { get; set; }

        [JsonProperty("bi", NullValueHandling = NullValueHandling.Ignore)]
        public long Bi { get; set; }

        [JsonProperty("newfans", NullValueHandling = NullValueHandling.Ignore)]
        public long Newfans { get; set; }

        [JsonProperty("unreadmsg", NullValueHandling = NullValueHandling.Ignore)]
        public Unreadmsg Unreadmsg { get; set; }

        [JsonProperty("group")]
        public object Group { get; set; }

        [JsonProperty("notice", NullValueHandling = NullValueHandling.Ignore)]
        public long Notice { get; set; }

        [JsonProperty("photo", NullValueHandling = NullValueHandling.Ignore)]
        public long Photo { get; set; }

        [JsonProperty("msgbox", NullValueHandling = NullValueHandling.Ignore)]
        public long Msgbox { get; set; }
    }
}