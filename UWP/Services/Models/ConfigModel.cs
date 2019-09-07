using Newtonsoft.Json;
using System.Collections.Generic;

namespace WeiPo.Services.Models
{
    public class ConfigModel
    {
        [JsonProperty("login", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Login { get; set; }

        [JsonProperty("st", NullValueHandling = NullValueHandling.Ignore)]
        public string St { get; set; }

        [JsonProperty("uid", NullValueHandling = NullValueHandling.Ignore)]
        public string Uid { get; set; }
    }

    public partial class EmojiResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("data")]
        public EmojiData Data { get; set; }
    }

    public partial class EmojiData
    {
        [JsonProperty("usual")]
        public Dictionary<string, EmojiModel[]> Usual { get; set; }

        [JsonProperty("more")]
        public Dictionary<string, EmojiModel[]> More { get; set; }

        [JsonProperty("brand")]
        public Brand Brand { get; set; }
    }

    public partial class Brand
    {
        [JsonProperty("norm")]
        public Dictionary<string, EmojiModel[]> Norm { get; set; }
    }

    public partial class EmojiModel
    {
        [JsonProperty("phrase")]
        public string Phrase { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("hot")]
        public bool Hot { get; set; }

        [JsonProperty("common")]
        public bool Common { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonIgnore]
        public string IconUrl => $"https:{Icon}";

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("picid")]
        public string Picid { get; set; }
    }
}