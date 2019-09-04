using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class Actionlog
    {
        [JsonProperty("act_code", NullValueHandling = NullValueHandling.Ignore)]
        public string ActCode { get; set; }

        [JsonProperty("cardid", NullValueHandling = NullValueHandling.Ignore)]
        public string Cardid { get; set; }

        [JsonProperty("oid", NullValueHandling = NullValueHandling.Ignore)]
        public string Oid { get; set; }

        [JsonProperty("featurecode", NullValueHandling = NullValueHandling.Ignore)]
        public string Featurecode { get; set; }

        [JsonProperty("mark", NullValueHandling = NullValueHandling.Ignore)]
        public string Mark { get; set; }

        [JsonProperty("ext", NullValueHandling = NullValueHandling.Ignore)]
        public string Ext { get; set; }

        [JsonProperty("uicode", NullValueHandling = NullValueHandling.Ignore)]
        public string Uicode { get; set; }

        [JsonProperty("luicode", NullValueHandling = NullValueHandling.Ignore)]
        public string Luicode { get; set; }

        [JsonProperty("fid", NullValueHandling = NullValueHandling.Ignore)]
        public string Fid { get; set; }

        [JsonProperty("lfid", NullValueHandling = NullValueHandling.Ignore)]
        public string Lfid { get; set; }

        [JsonProperty("lcardid", NullValueHandling = NullValueHandling.Ignore)]
        public string Lcardid { get; set; }
    }
}