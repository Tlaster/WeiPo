using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class CommentManageInfo
    {
        [JsonProperty("comment_permission_type")]
        public long CommentPermissionType { get; set; }

        [JsonProperty("approval_comment_type")]
        public long ApprovalCommentType { get; set; }

        [JsonProperty("comment_manage_button")]
        public long CommentManageButton { get; set; }
    }
}