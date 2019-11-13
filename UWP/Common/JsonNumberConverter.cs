using System;
using System.Numerics;
using Newtonsoft.Json;

namespace WeiPo.Common
{
    public class JsonNumberConverter : JsonConverter
    {
        private readonly JsonSerializer _defaultSerializer = new JsonSerializer();

        public override bool CanConvert(Type objectType)
        {
            var type = Nullable.GetUnderlyingType(objectType) ?? objectType;
            return type == typeof(long)
                   || type == typeof(ulong)
                   || type == typeof(int)
                   || type == typeof(uint)
                   || type == typeof(short)
                   || type == typeof(ushort)
                   || type == typeof(byte)
                   || type == typeof(sbyte)
                   || type == typeof(BigInteger);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Integer:
                case JsonToken.Float: // Accepts numbers like 4.00
                case JsonToken.Null:
                    return _defaultSerializer.Deserialize(reader, objectType);
                case JsonToken.String:
                    if (string.IsNullOrEmpty(reader.Value as string))
                    {
                        return Convert.ChangeType(0, objectType);
                    }

                    try
                    {
                        return Convert.ChangeType(reader.Value, objectType);
                    }
                    catch (FormatException e)
                    {
                        return Convert.ChangeType(0, objectType);
                    } 
                default:
                    throw new JsonSerializationException(
                        $"Token \"{reader.Value}\" of type {reader.TokenType} was not a JSON integer");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            serializer.Serialize(writer, value.ToString());
        }
    }
}