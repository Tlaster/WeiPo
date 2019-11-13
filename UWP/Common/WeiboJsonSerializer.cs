using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WeiPo.Common
{
    [Serializable]
    public class WeiboException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public WeiboException()
        {
        }

        public WeiboException(string message) : base(message)
        {
        }

        public WeiboException(string message, Exception inner) : base(message, inner)
        {
        }

        protected WeiboException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }

    public class WeiboJsonSerializerSetting
    {

        public static JsonSerializerSettings Settings { get; } = new JsonSerializerSettings
        {
            Converters =
            {
                new JsonNumberConverter()
            },
            Error = (sender, args) =>
            {
                var currentError = args.ErrorContext.Error.Message;
                Debug.WriteLine(currentError);
                args.ErrorContext.Handled = true;
            },
            NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
        };

        public static JsonSerializer Serializer { get; } = JsonSerializer.Create(Settings);
    }

    public class WeiboJsonSerializer : ISerializer
    {
        public WeiboJsonSerializer(JsonSerializerSettings settings)
        {
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, WeiboJsonSerializerSetting.Settings);
        }

        public T Deserialize<T>(string s)
        {
            var obj = JsonConvert.DeserializeObject<JToken>(s, WeiboJsonSerializerSetting.Settings);
            CheckIfError(obj);
            return obj.ToObject<T>(WeiboJsonSerializerSetting.Serializer);
        }

        public T Deserialize<T>(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var obj = WeiboJsonSerializerSetting.Serializer.Deserialize<JToken>(jsonTextReader);
            if (obj == null)
            {
                return default;
            }
            CheckIfError(obj);
            return obj.ToObject<T>(WeiboJsonSerializerSetting.Serializer);
        }

        private static void CheckIfError(JToken obj)
        {
            if (obj is JObject jobj && jobj.ContainsKey("ok") && jobj.Value<int>("ok") != 1)
            {
                throw new WeiboException(jobj.ToString());
            }
        }
    }
}