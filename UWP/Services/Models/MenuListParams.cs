using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class MenuListParams
    {
        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }
    }
}