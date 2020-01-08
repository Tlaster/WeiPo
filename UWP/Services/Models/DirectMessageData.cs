using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class DirectMessageData
    {
        [JsonProperty("msgs")]
        public List<Msg> Msgs { get; set; }

        [JsonProperty("users")]
        public Dictionary<string, UserModel> Users { get; set; }

        [JsonProperty("total_number")]
        public long TotalNumber { get; set; }

        [JsonProperty("following")]
        public bool Following { get; set; }

        [JsonProperty("last_read_mid")]
        public long LastReadMid { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}