using System;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class DarwinTag
    {
        [JsonProperty("object_type", NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectType { get; set; }

        [JsonProperty("object_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectId { get; set; }

        [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("enterprise_uid")]
        public object EnterpriseUid { get; set; }

        [JsonProperty("pc_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri PcUrl { get; set; }

        [JsonProperty("mapi_url", NullValueHandling = NullValueHandling.Ignore)]
        public string MapiUrl { get; set; }

        [JsonProperty("bd_object_type", NullValueHandling = NullValueHandling.Ignore)]
        public string BdObjectType { get; set; }
    }
}