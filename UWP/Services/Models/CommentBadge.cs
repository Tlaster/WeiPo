using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class CommentBadge
    {
        [JsonProperty("pic_url")] public string PicUrl { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("length")] public double Length { get; set; }

        [JsonProperty("actionlog")] public Actionlog Actionlog { get; set; }

        [JsonProperty("scheme")] public string Scheme { get; set; }
    }
}