using System;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class UploadPicModel
    {
        [JsonProperty("pic_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PicId { get; set; }

        [JsonProperty("thumbnail_pic", NullValueHandling = NullValueHandling.Ignore)]
        public string ThumbnailPic { get; set; }

        [JsonProperty("bmiddle_pic", NullValueHandling = NullValueHandling.Ignore)]
        public string BmiddlePic { get; set; }

        [JsonProperty("original_pic", NullValueHandling = NullValueHandling.Ignore)]
        public string OriginalPic { get; set; }
    }
}