using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class LongTextModel
    {
        [JsonProperty("ok")]
        public long Ok { get; set; }

        [JsonProperty("longTextContent")]
        public string LongTextContent { get; set; }

        [JsonProperty("reposts_count")]
        public long RepostsCount { get; set; }

        [JsonProperty("comments_count")]
        public long CommentsCount { get; set; }

        [JsonProperty("attitudes_count")]
        public long AttitudesCount { get; set; }
    }
}