using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class Attachment
    {
        [JsonProperty("fid")]
        public long Fid { get; set; }

        [JsonProperty("vfid")]
        public long Vfid { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("filesize")]
        public string Filesize { get; set; }

        [JsonProperty("original_image")]
        public OriginalImage OriginalImage { get; set; }

        [JsonProperty("thumbnail")]
        public OriginalImage Thumbnail { get; set; }
    }
}