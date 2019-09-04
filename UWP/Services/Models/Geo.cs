using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class Geo
    {
        [JsonProperty("width")] public long Width { get; set; }

        [JsonProperty("height")] public long Height { get; set; }

        [JsonProperty("croped")] public bool Croped { get; set; }
    }
}