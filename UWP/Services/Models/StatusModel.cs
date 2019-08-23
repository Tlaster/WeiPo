using System;
using System.Collections.Generic;
using Windows.Graphics.Printing.OptionDetails;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public partial class StatusModel
    {
        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("mid", NullValueHandling = NullValueHandling.Ignore)]
        public string Mid { get; set; }

        [JsonProperty("can_edit", NullValueHandling = NullValueHandling.Ignore)]
        public bool CanEdit { get; set; }

        [JsonProperty("show_additional_indication", NullValueHandling = NullValueHandling.Ignore)]
        public long ShowAdditionalIndication { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("textLength", NullValueHandling = NullValueHandling.Ignore)]
        public long TextLength { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty("favorited", NullValueHandling = NullValueHandling.Ignore)]
        public bool Favorited { get; set; }

        [JsonProperty("pic_ids", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> PicIds { get; set; }

        [JsonProperty("pic_types", NullValueHandling = NullValueHandling.Ignore)]
        public string PicTypes { get; set; }

        [JsonProperty("is_paid", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsPaid { get; set; }

        [JsonProperty("mblog_vip_type", NullValueHandling = NullValueHandling.Ignore)]
        public long MblogVipType { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserModel User { get; set; }

        [JsonProperty("reposts_count", NullValueHandling = NullValueHandling.Ignore)]
        public long RepostsCount { get; set; }

        [JsonProperty("comments_count", NullValueHandling = NullValueHandling.Ignore)]
        public long CommentsCount { get; set; }

        [JsonProperty("attitudes_count", NullValueHandling = NullValueHandling.Ignore)]
        public long AttitudesCount { get; set; }

        [JsonProperty("pending_approval_count", NullValueHandling = NullValueHandling.Ignore)]
        public long PendingApprovalCount { get; set; }

        [JsonProperty("isLongText", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsLongText { get; set; }

        [JsonProperty("reward_exhibition_type", NullValueHandling = NullValueHandling.Ignore)]
        public long RewardExhibitionType { get; set; }

        [JsonProperty("hide_flag", NullValueHandling = NullValueHandling.Ignore)]
        public long HideFlag { get; set; }

        [JsonProperty("visible", NullValueHandling = NullValueHandling.Ignore)]
        public Visible Visible { get; set; }

        [JsonProperty("darwin_tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> DarwinTags { get; set; }

        [JsonProperty("mblogtype", NullValueHandling = NullValueHandling.Ignore)]
        public long Mblogtype { get; set; }

        [JsonProperty("more_info_type", NullValueHandling = NullValueHandling.Ignore)]
        public long MoreInfoType { get; set; }

        [JsonProperty("cardid", NullValueHandling = NullValueHandling.Ignore)]
        public string Cardid { get; set; }

        [JsonProperty("number_display_strategy", NullValueHandling = NullValueHandling.Ignore)]
        public NumberDisplayStrategy NumberDisplayStrategy { get; set; }

        [JsonProperty("content_auth", NullValueHandling = NullValueHandling.Ignore)]
        public long ContentAuth { get; set; }

        [JsonProperty("pic_num", NullValueHandling = NullValueHandling.Ignore)]
        public long PicNum { get; set; }

        [JsonProperty("bid", NullValueHandling = NullValueHandling.Ignore)]
        public string Bid { get; set; }

        [JsonProperty("retweeted_status", NullValueHandling = NullValueHandling.Ignore)]
        public StatusModel RetweetedStatus { get; set; }

        [JsonProperty("raw_text", NullValueHandling = NullValueHandling.Ignore)]
        public string RawText { get; set; }

        [JsonProperty("pid", NullValueHandling = NullValueHandling.Ignore)]
        public long Pid { get; set; }

        [JsonProperty("pidstr", NullValueHandling = NullValueHandling.Ignore)]
        public string Pidstr { get; set; }

        [JsonProperty("pic_focus_point", NullValueHandling = NullValueHandling.Ignore)]
        public List<PicFocusPoint> PicFocusPoint { get; set; }

        [JsonProperty("pic_rectangle_object", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> PicRectangleObject { get; set; }

        [JsonProperty("pic_flag", NullValueHandling = NullValueHandling.Ignore)]
        public long PicFlag { get; set; }

        [JsonProperty("thumbnail_pic", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ThumbnailPic { get; set; }

        [JsonProperty("bmiddle_pic", NullValueHandling = NullValueHandling.Ignore)]
        public Uri BmiddlePic { get; set; }

        [JsonProperty("original_pic", NullValueHandling = NullValueHandling.Ignore)]
        public Uri OriginalPic { get; set; }

        [JsonProperty("page_info", NullValueHandling = NullValueHandling.Ignore)]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("pics", NullValueHandling = NullValueHandling.Ignore)]
        public List<Pic> Pics { get; set; }

        [JsonProperty("reward_scheme", NullValueHandling = NullValueHandling.Ignore)]
        public string RewardScheme { get; set; }
    }
}