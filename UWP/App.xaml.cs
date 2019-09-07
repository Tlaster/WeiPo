using System.Linq;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Flurl.Http;
using Newtonsoft.Json;
using WeiPo.Common;
using WeiPo.Services;
using Flurl.Http.Configuration;
using Flurl.Util;
using Flurl;
using System;
using System.IO;

namespace WeiPo
{
    public class WeiboUrlEncodedSerializer : ISerializer
    {
        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public string Serialize(object obj)
        {
            if (obj == null)
                return null;

            var qp = new QueryParamCollection();
            foreach (var kv in obj.ToKeyValuePairs())
                qp.Merge(kv.Key, kv.Value, false, Flurl.NullValueHandling.Ignore);
            return qp.ToString(true);
        }

        /// <summary>
        /// Deserializes the specified s.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s">The s.</param>
        /// <exception cref="NotImplementedException">Deserializing to UrlEncoded not supported.</exception>
        public T Deserialize<T>(string s)
        {
            throw new NotImplementedException("Deserializing to UrlEncoded is not supported.");
        }

        /// <summary>
        /// Deserializes the specified stream.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">The stream.</param>
        /// <exception cref="NotImplementedException">Deserializing to UrlEncoded not supported.</exception>
        public T Deserialize<T>(Stream stream)
        {
            throw new NotImplementedException("Deserializing to UrlEncoded is not supported.");
        }
    }
    public sealed partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            FlurlHttp.Configure(settings =>
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonNumberConverter()
                    },
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
                settings.JsonSerializer = new WeiboJsonSerializer(jsonSettings);
                //settings.UrlEncodedSerializer = new WeiboUrlEncodedSerializer();
                settings.BeforeCall = call =>
                {
                    call.Request.Headers.Add("Cookie",
                        string.Join(";",
                            Singleton<Api>.Instance.GetCookies().Select(it => $"{it.Key}={it.Value}")));
                };
            });
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (!(Window.Current.Content is RootView))
            {
                Window.Current.Content = new RootView();
            }

            if (e.PrelaunchActivated == false)
            {
                Window.Current.Activate();
            }
        }
    }
}