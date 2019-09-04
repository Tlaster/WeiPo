using System;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    internal class ParseStringConverter : JsonConverter
    {
        public static readonly ParseStringConverter Singleton = new ParseStringConverter();

        public override bool CanConvert(Type t)
        {
            return t == typeof(long) || t == typeof(long);
        }

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var value = serializer.Deserialize<string>(reader);
            long l;
            if (long.TryParse(value, out l))
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

            var value = (long) untypedValue;
            serializer.Serialize(writer, value.ToString());
        }
    }
}