using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class WeiboResponse<T>
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }

        [JsonProperty("ok", NullValueHandling = NullValueHandling.Ignore)]
        public long Ok { get; set; }

        [JsonProperty("http_code", NullValueHandling = NullValueHandling.Ignore)]
        public long HttpCode { get; set; }
    }
}
