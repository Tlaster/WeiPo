using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
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

        public WeiboJsonSerializer(JsonSerializerSettings settings)
        {
            this._settings = settings;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, this._settings);
        }

        public T Deserialize<T>(string s)
        {
            var obj = JsonConvert.DeserializeObject<JToken>(s, this._settings);
            CheckIfError(obj);
            return obj.ToObject<T>();
        }

        private static void CheckIfError(JToken obj)
        {
            if (obj is JObject jobj && jobj.ContainsKey("ok") && jobj.Value<int>("ok") != 1)
            {
                throw new WeiboException(jobj.ToString());
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var obj = JsonSerializer.CreateDefault(this._settings).Deserialize<JToken>(jsonTextReader);
            CheckIfError(obj);
            return obj.ToObject<T>();
        }
    }
}
