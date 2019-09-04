using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class PicFocusPoint
    {
        [JsonProperty("focus_point", NullValueHandling = NullValueHandling.Ignore)]
        public FocusPoint FocusPoint { get; set; }

        [JsonProperty("pic_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PicId { get; set; }
    }
}