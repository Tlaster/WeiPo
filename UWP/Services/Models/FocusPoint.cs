using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class FocusPoint
    {
        [JsonProperty("left", NullValueHandling = NullValueHandling.Ignore)]
        public double Left { get; set; }

        [JsonProperty("top", NullValueHandling = NullValueHandling.Ignore)]
        public double Top { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public double Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public double Height { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public long Type { get; set; }
    }
}