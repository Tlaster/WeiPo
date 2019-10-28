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
}