using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class PicWall
    {
        [JsonProperty("pic_small")]
        public string PicSmall { get; set; }

        [JsonProperty("pic_middle")]
        public string PicMiddle { get; set; }

        [JsonProperty("pic_big")]
        public string PicBig { get; set; }

        [JsonProperty("mblog")]
        public StatusModel Mblog { get; set; }

        [JsonProperty("object_id")]
        public string ObjectId { get; set; }

        [JsonProperty("scheme")]
        public string Scheme { get; set; }

        [JsonProperty("actionlog")]
        public string Actionlog { get; set; }

        [JsonProperty("photo_tag")]
        public long PhotoTag { get; set; }

        [JsonProperty("pic_id")]
        public string PicId { get; set; }

        [JsonProperty("savedisable")]
        public long Savedisable { get; set; }
    }
}