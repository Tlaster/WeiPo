using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class StoryModel
    {
        [JsonProperty("object_id")]
        public string ObjectId { get; set; }

        [JsonProperty("object_type")]
        public string ObjectType { get; set; }

        [JsonProperty("object")]
        public StoryObject StoryObject { get; set; }
    }
}