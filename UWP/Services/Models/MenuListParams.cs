using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class MenuListParams
    {
        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }
    }
}