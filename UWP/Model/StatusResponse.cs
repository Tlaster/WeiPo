using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WeiPo.Model
{

    public partial class StatusResponse
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Data Data { get; set; }

        [JsonProperty("ok", NullValueHandling = NullValueHandling.Ignore)]
        public long Ok { get; set; }

        [JsonProperty("http_code", NullValueHandling = NullValueHandling.Ignore)]
        public long HttpCode { get; set; }
    }

    public partial class Data
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

    public partial class NumberDisplayStrategy
    {
        [JsonProperty("apply_scenario_flag", NullValueHandling = NullValueHandling.Ignore)]
        public long ApplyScenarioFlag { get; set; }

        [JsonProperty("display_text_min_number", NullValueHandling = NullValueHandling.Ignore)]
        public long DisplayTextMinNumber { get; set; }

        [JsonProperty("display_text", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayText { get; set; }
    }

    public partial class PageInfo
    {
        [JsonProperty("object_type", NullValueHandling = NullValueHandling.Ignore)]
        public long ObjectType { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("page_pic", NullValueHandling = NullValueHandling.Ignore)]
        public PagePic PagePic { get; set; }

        [JsonProperty("page_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri PageUrl { get; set; }

        [JsonProperty("page_title", NullValueHandling = NullValueHandling.Ignore)]
        public string PageTitle { get; set; }

        [JsonProperty("content1", NullValueHandling = NullValueHandling.Ignore)]
        public string Content1 { get; set; }

        [JsonProperty("object_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectId { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("content2", NullValueHandling = NullValueHandling.Ignore)]
        public string Content2 { get; set; }

        [JsonProperty("video_orientation", NullValueHandling = NullValueHandling.Ignore)]
        public string VideoOrientation { get; set; }

        [JsonProperty("play_count", NullValueHandling = NullValueHandling.Ignore)]
        public string PlayCount { get; set; }

        [JsonProperty("media_info", NullValueHandling = NullValueHandling.Ignore)]
        public MediaInfo MediaInfo { get; set; }

        [JsonProperty("urls", NullValueHandling = NullValueHandling.Ignore)]
        public Urls Urls { get; set; }

        [JsonProperty("video_details", NullValueHandling = NullValueHandling.Ignore)]
        public VideoDetails VideoDetails { get; set; }
    }

    public partial class MediaInfo
    {
        [JsonProperty("stream_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri StreamUrl { get; set; }

        [JsonProperty("stream_url_hd", NullValueHandling = NullValueHandling.Ignore)]
        public Uri StreamUrlHd { get; set; }

        [JsonProperty("duration", NullValueHandling = NullValueHandling.Ignore)]
        public double Duration { get; set; }
    }

    public partial class PagePic
    {
        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long Height { get; set; }
    }

    public partial class Urls
    {
        [JsonProperty("mp4_720p_mp4", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Mp4720PMp4 { get; set; }

        [JsonProperty("mp4_hd_mp4", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Mp4HdMp4 { get; set; }

        [JsonProperty("mp4_ld_mp4", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Mp4LdMp4 { get; set; }
    }

    public partial class VideoDetails
    {
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public long Size { get; set; }

        [JsonProperty("bitrate", NullValueHandling = NullValueHandling.Ignore)]
        public long Bitrate { get; set; }

        [JsonProperty("label", NullValueHandling = NullValueHandling.Ignore)]
        public string Label { get; set; }

        [JsonProperty("prefetch_size", NullValueHandling = NullValueHandling.Ignore)]
        public long PrefetchSize { get; set; }
    }

    public partial class PicFocusPoint
    {
        [JsonProperty("focus_point", NullValueHandling = NullValueHandling.Ignore)]
        public FocusPoint FocusPoint { get; set; }

        [JsonProperty("pic_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PicId { get; set; }
    }

    public partial class FocusPoint
    {
        [JsonProperty("left", NullValueHandling = NullValueHandling.Ignore)]
        public double Left { get; set; }

        [JsonProperty("top", NullValueHandling = NullValueHandling.Ignore)]
        public double Top { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public double Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public double Height { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public long Type { get; set; }
    }

    public partial class Pic
    {
        [JsonProperty("pid", NullValueHandling = NullValueHandling.Ignore)]
        public string Pid { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty("geo", NullValueHandling = NullValueHandling.Ignore)]
        public PicGeo Geo { get; set; }

        [JsonProperty("large", NullValueHandling = NullValueHandling.Ignore)]
        public Large Large { get; set; }
    }

    public partial class PicGeo
    {
        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long Height { get; set; }

        [JsonProperty("croped", NullValueHandling = NullValueHandling.Ignore)]
        public bool Croped { get; set; }
    }

    public partial class Large
    {
        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public string Size { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("geo", NullValueHandling = NullValueHandling.Ignore)]
        public LargeGeo Geo { get; set; }
    }

    public partial class LargeGeo
    {
        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Width { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Height { get; set; }

        [JsonProperty("croped", NullValueHandling = NullValueHandling.Ignore)]
        public bool Croped { get; set; }
    }

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

    public partial class PicRectangleObject
    {
        [JsonProperty("rectangle_objects", NullValueHandling = NullValueHandling.Ignore)]
        public List<FocusPoint> RectangleObjects { get; set; }

        [JsonProperty("pic_id", NullValueHandling = NullValueHandling.Ignore)]
        public string PicId { get; set; }
    }

    public partial class UserModel
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public long Id { get; set; }

        [JsonProperty("screen_name", NullValueHandling = NullValueHandling.Ignore)]
        public string ScreenName { get; set; }

        [JsonProperty("profile_image_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ProfileImageUrl { get; set; }

        [JsonProperty("profile_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri ProfileUrl { get; set; }

        [JsonProperty("statuses_count", NullValueHandling = NullValueHandling.Ignore)]
        public long StatusesCount { get; set; }

        [JsonProperty("verified", NullValueHandling = NullValueHandling.Ignore)]
        public bool Verified { get; set; }

        [JsonProperty("verified_type", NullValueHandling = NullValueHandling.Ignore)]
        public long VerifiedType { get; set; }

        [JsonProperty("verified_type_ext", NullValueHandling = NullValueHandling.Ignore)]
        public long VerifiedTypeExt { get; set; }

        [JsonProperty("verified_reason", NullValueHandling = NullValueHandling.Ignore)]
        public string VerifiedReason { get; set; }

        [JsonProperty("close_blue_v", NullValueHandling = NullValueHandling.Ignore)]
        public bool CloseBlueV { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("gender", NullValueHandling = NullValueHandling.Ignore)]
        public string Gender { get; set; }

        [JsonProperty("mbtype", NullValueHandling = NullValueHandling.Ignore)]
        public long Mbtype { get; set; }

        [JsonProperty("urank", NullValueHandling = NullValueHandling.Ignore)]
        public long Urank { get; set; }

        [JsonProperty("mbrank", NullValueHandling = NullValueHandling.Ignore)]
        public long Mbrank { get; set; }

        [JsonProperty("follow_me", NullValueHandling = NullValueHandling.Ignore)]
        public bool FollowMe { get; set; }

        [JsonProperty("following", NullValueHandling = NullValueHandling.Ignore)]
        public bool Following { get; set; }

        [JsonProperty("followers_count", NullValueHandling = NullValueHandling.Ignore)]
        public long FollowersCount { get; set; }

        [JsonProperty("follow_count", NullValueHandling = NullValueHandling.Ignore)]
        public long FollowCount { get; set; }

        [JsonProperty("cover_image_phone", NullValueHandling = NullValueHandling.Ignore)]
        public Uri CoverImagePhone { get; set; }

        [JsonProperty("avatar_hd", NullValueHandling = NullValueHandling.Ignore)]
        public Uri AvatarHd { get; set; }

        [JsonProperty("like", NullValueHandling = NullValueHandling.Ignore)]
        public bool Like { get; set; }

        [JsonProperty("like_me", NullValueHandling = NullValueHandling.Ignore)]
        public bool LikeMe { get; set; }

        [JsonProperty("badge", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, long> Badge { get; set; }
    }

    public partial class Visible
    {
        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public long Type { get; set; }

        [JsonProperty("list_id", NullValueHandling = NullValueHandling.Ignore)]
        public long ListId { get; set; }
    }


    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
