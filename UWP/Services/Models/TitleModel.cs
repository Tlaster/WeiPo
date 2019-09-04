using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class TitleModel
    {
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("base_color", NullValueHandling = NullValueHandling.Ignore)]
        public long? BaseColor { get; set; }
    }
}