using System;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class PicGeoConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
            }
            else
            {
                serializer.Serialize(writer, value as PicGeo);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.TokenType switch
            {
                JsonToken.Null => null,
                JsonToken.Boolean => null,
                _ => serializer.Deserialize<PicGeo>(reader)
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PicGeo);
        }
    }
}