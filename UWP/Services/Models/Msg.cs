using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class Msg
    {
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("dm_type")]
        public long DmType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("msg_status")]
        public long MsgStatus { get; set; }

        [JsonProperty("media_type")]
        public long MediaType { get; set; }

        [JsonProperty("recipient_id")]
        public long RecipientId { get; set; }

        [JsonProperty("recipient_screen_name")]
        public string RecipientScreenName { get; set; }

        [JsonProperty("sender_id")]
        public long SenderId { get; set; }

        [JsonProperty("sender_screen_name")]
        public string SenderScreenName { get; set; }

        [JsonProperty("attachment", NullValueHandling = NullValueHandling.Ignore)]
        public Attachment Attachment { get; set; }
    }
}