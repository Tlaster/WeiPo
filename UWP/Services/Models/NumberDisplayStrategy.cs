using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class NumberDisplayStrategy
    {
        [JsonProperty("apply_scenario_flag", NullValueHandling = NullValueHandling.Ignore)]
        public long ApplyScenarioFlag { get; set; }

        [JsonProperty("display_text_min_number", NullValueHandling = NullValueHandling.Ignore)]
        public long DisplayTextMinNumber { get; set; }

        [JsonProperty("display_text", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayText { get; set; }
    }
}