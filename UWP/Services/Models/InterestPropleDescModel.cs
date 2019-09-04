using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class InterestPropleDescModel
    {
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public string Desc { get; set; }

        [JsonProperty("card_type", NullValueHandling = NullValueHandling.Ignore)]
        public long CardType { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }

        [JsonProperty("actionlog", NullValueHandling = NullValueHandling.Ignore)]
        public Actionlog Actionlog { get; set; }

        [JsonProperty("display_arrow", NullValueHandling = NullValueHandling.Ignore)]
        public long DisplayArrow { get; set; }

        [JsonProperty("title_extra_text", NullValueHandling = NullValueHandling.Ignore)]
        public string TitleExtraText { get; set; }
    }
}