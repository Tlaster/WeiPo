using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class ConfigModel
    {
        [JsonProperty("login", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Login { get; set; }

        [JsonProperty("st", NullValueHandling = NullValueHandling.Ignore)]
        public string St { get; set; }

        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public string Uid { get; set; }
    }
}