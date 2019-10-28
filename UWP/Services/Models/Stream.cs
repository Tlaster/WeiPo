using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class Stream
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}