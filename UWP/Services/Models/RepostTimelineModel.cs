using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class RepostTimelineModel
    {
        [JsonProperty("data")]
        public List<StatusModel> Data { get; set; }

        [JsonProperty("total_number")]
        public long TotalNumber { get; set; }

        [JsonProperty("hot_total_number")]
        public long HotTotalNumber { get; set; }

        [JsonProperty("max")]
        public long Max { get; set; }
    }
}