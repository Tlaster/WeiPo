using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class TimelineData
    {
        [JsonProperty("statuses", NullValueHandling = NullValueHandling.Ignore)]
        public List<StatusModel> Statuses { get; set; }

        [JsonProperty("advertises", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Advertises { get; set; }

        [JsonProperty("ad", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Ad { get; set; }

        [JsonProperty("filtered_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> FilteredIds { get; set; }

        [JsonProperty("hasvisible", NullValueHandling = NullValueHandling.Ignore)]
        public bool Hasvisible { get; set; }

        [JsonProperty("previous_cursor", NullValueHandling = NullValueHandling.Ignore)]
        public long PreviousCursor { get; set; }

        [JsonProperty("next_cursor", NullValueHandling = NullValueHandling.Ignore)]
        public long NextCursor { get; set; }

        [JsonProperty("previous_cursor_str", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long PreviousCursorStr { get; set; }

        [JsonProperty("next_cursor_str", NullValueHandling = NullValueHandling.Ignore)]
        public string NextCursorStr { get; set; }

        [JsonProperty("total_number", NullValueHandling = NullValueHandling.Ignore)]
        public long TotalNumber { get; set; }

        [JsonProperty("interval", NullValueHandling = NullValueHandling.Ignore)]
        public long Interval { get; set; }

        [JsonProperty("uve_blank", NullValueHandling = NullValueHandling.Ignore)]
        public long UveBlank { get; set; }

        [JsonProperty("since_id", NullValueHandling = NullValueHandling.Ignore)]
        public long SinceId { get; set; }

        [JsonProperty("since_id_str", NullValueHandling = NullValueHandling.Ignore)]
        public string SinceIdStr { get; set; }

        [JsonProperty("max_id", NullValueHandling = NullValueHandling.Ignore)]
        public long MaxId { get; set; }

        [JsonProperty("max_id_str", NullValueHandling = NullValueHandling.Ignore)]
        public string MaxIdStr { get; set; }

        [JsonProperty("has_unread", NullValueHandling = NullValueHandling.Ignore)]
        public long HasUnread { get; set; }
    }
}