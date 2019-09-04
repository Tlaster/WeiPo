using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class InterestPeopleModel
    {
        [JsonProperty("card_type", NullValueHandling = NullValueHandling.Ignore)]
        public long? CardType { get; set; }

        [JsonProperty("itemid", NullValueHandling = NullValueHandling.Ignore)]
        public string Itemid { get; set; }

        [JsonProperty("scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string Scheme { get; set; }

        [JsonProperty("background_color", NullValueHandling = NullValueHandling.Ignore)]
        public long? BackgroundColor { get; set; }

        [JsonProperty("recom_remark", NullValueHandling = NullValueHandling.Ignore)]
        public string RecomRemark { get; set; }

        [JsonProperty("desc1", NullValueHandling = NullValueHandling.Ignore)]
        public string Desc1 { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserModel User { get; set; }

        [JsonProperty("actionlog", NullValueHandling = NullValueHandling.Ignore)]
        public Actionlog Actionlog { get; set; }

        [JsonProperty("buttons", NullValueHandling = NullValueHandling.Ignore)]
        public List<ButtonModel> Buttons { get; set; }
    }
}