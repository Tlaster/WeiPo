using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Flurl.Http;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using WeiPo.Common;
using WeiPo.Services;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace WeiPo
{
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
                    Error = (sender, args) =>
                    {
                        var currentError = args.ErrorContext.Error.Message;
                        Debug.WriteLine(currentError);
                        args.ErrorContext.Handled = true;
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
            ImageCache.Instance.InitializeAsync(httpMessageHandler: new WeiboHttpClientHandler());

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