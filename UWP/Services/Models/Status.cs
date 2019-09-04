using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class Status
    {
        [JsonProperty("comment_manage_info")]
        public CommentManageInfo CommentManageInfo { get; set; }
    }
}