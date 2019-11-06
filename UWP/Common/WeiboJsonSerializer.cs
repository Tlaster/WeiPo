using System;
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

    public class WeiboJsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;
        private readonly JsonSerializer _defaultSerializer;

        public WeiboJsonSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
            _defaultSerializer = JsonSerializer.CreateDefault(_settings);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T Deserialize<T>(string s)
        {
            var obj = JsonConvert.DeserializeObject<JToken>(s, _settings);
            CheckIfError(obj);
            return obj.ToObject<T>(_defaultSerializer);
        }

        public T Deserialize<T>(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var obj = _defaultSerializer.Deserialize<JToken>(jsonTextReader);
            if (obj == null)
            {
                return default;
            }
            CheckIfError(obj);
            return obj.ToObject<T>(_defaultSerializer);
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