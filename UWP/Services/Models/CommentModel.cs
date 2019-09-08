using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    internal class CommentsConverter : JsonConverter
    {
        public static readonly ParseStringConverter Singleton = new ParseStringConverter();

        public override bool CanConvert(Type t)
        {
            return t == typeof(List<>);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            return reader.TokenType switch
            {
                JsonToken.Null => null,
                JsonToken.Boolean => null,
                _ => serializer.Deserialize<List<CommentModel>>(reader)
            };
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            serializer.Serialize(writer, untypedValue as List<CommentModel>);
        }
    }
    public class CommentModel : ICanReply
    {
        [JsonProperty("max_id")] public long MaxId { get; set; }

        [JsonProperty("more_info_type")] public long MoreInfoType { get; set; }

        [JsonProperty("pic")] public Pic Pic { get; set; }

        [JsonProperty("readtimetype")] public string Readtimetype { get; set; }

        [JsonProperty("comment_badge")] public CommentBadge[] CommentBadge { get; set; }

        [JsonProperty("isLikedByMblogAuthor", NullValueHandling = NullValueHandling.Ignore)]
        public bool IsLikedByMblogAuthor { get; set; }

        [JsonProperty("total_number", NullValueHandling = NullValueHandling.Ignore)]
        public long TotalNumber { get; set; }

        [JsonProperty("comments", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(CommentsConverter))]
        public List<CommentModel> Comments { get; set; }

        [JsonProperty("like_count", NullValueHandling = NullValueHandling.Ignore)]
        public long LikeCount { get; set; }

        [JsonProperty("rootid", NullValueHandling = NullValueHandling.Ignore)]
        public string Rootid { get; set; }

        [JsonProperty("disable_reply", NullValueHandling = NullValueHandling.Ignore)]
        public long DisableReply { get; set; }

        [JsonProperty("created_at", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedAt { get; set; }

        [JsonProperty("floor_number", NullValueHandling = NullValueHandling.Ignore)]
        public long FloorNumber { get; set; }

        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }

        [JsonProperty("rootidstr", NullValueHandling = NullValueHandling.Ignore)]
        public string Rootidstr { get; set; }

        [JsonProperty("reply_count", NullValueHandling = NullValueHandling.Ignore)]
        public long ReplyCount { get; set; }

        [JsonProperty("liked", NullValueHandling = NullValueHandling.Ignore)]
        public bool Liked { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        public UserModel User { get; set; }

        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public StatusModel Status { get; set; }

        [JsonProperty("bid", NullValueHandling = NullValueHandling.Ignore)]
        public string Bid { get; set; }

        [JsonProperty("mid", NullValueHandling = NullValueHandling.Ignore)]
        public string Mid { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
    }
}