using System;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class StoryObject
    {
        [JsonProperty("summary")]
        public object Summary { get; set; }

        [JsonProperty("author")]
        public UserModel Author { get; set; }

        [JsonProperty("stream")]
        public Stream Stream { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("image")]
        public Image Image { get; set; }
    }
}