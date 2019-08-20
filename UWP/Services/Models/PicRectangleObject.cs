using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class PicRectangleObject
    {
        [JsonProperty("rectangle_objects", NullValueHandling = NullValueHandling.Ignore)]
        public List<FocusPoint> RectangleObjects { get; set; }

        [JsonProperty("pic_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PicId { get; set; }
    }
}