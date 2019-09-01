using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class AttitudeModel
    {
        [JsonProperty("idStr", NullValueHandling = NullValueHandling.Ignore)]
        public string IdStr { get; set; }

        [JsonProperty("attitude_type", NullValueHandling = NullValueHandling.Ignore)]
        public long? AttitudeType { get; set; }

        [JsonProperty("source_allowclick", NullValueHandling = NullValueHandling.Ignore)]
        public long? SourceAllowclick { get; set; }

        [JsonProperty("attitude_mask", NullValueHandling = NullValueHandling.Ignore)]
        public long? AttitudeMask { get; set; }

        [JsonProperty("last_attitude", NullValueHandling = NullValueHandling.Ignore)]
        public string LastAttitude { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("source_type", NullValueHandling = NullValueHandling.Ignore)]
        public long? SourceType { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long? Id { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserModel User { get; set; }

        [JsonProperty("attitude", NullValueHandling = NullValueHandling.Ignore)]
        public string Attitude { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public StatusModel Status { get; set; }
    }
}