using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Flurl.Http;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Toolkit.Uwp.Helpers;
using Newtonsoft.Json;
using WeiPo.Common;
using WeiPo.Services;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Newtonsoft.Json.Linq;

namespace WeiPo
{
    public sealed partial class App
    {
        public App()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var id = ResourceLoader.GetForViewIndependentUse("AppCenter")?.GetString("AppCenterId");
            if (!string.IsNullOrEmpty(id))
            {
                AppCenter.Start(id,
                    typeof(Analytics), typeof(Crashes));
            }
            FlurlHttp.Configure(settings =>
            {
                settings.JsonSerializer = new WeiboJsonSerializer(WeiboJsonSerializerSetting.Settings);
                //settings.UrlEncodedSerializer = new WeiboUrlEncodedSerializer();
                settings.BeforeCall = call =>
                {
                    call.Request.Headers.Add("Cookie",
                        string.Join(";",
                            Singleton<Api>.Instance.GetCookies().Select(it => $"{it.Key}={it.Value}")));
                };
                settings.OnErrorAsync = async call =>
                {
                    if (call.Response.StatusCode == 403)
                    {
                        //maybe errno:100005
                        var json = await call.Response.GetStringAsync();
                        try
                        {
                            call.Request.Client.Settings.JsonSerializer.Deserialize<JObject>(json);
                        }
                        catch (FlurlParsingException e) when (e.InnerException is WeiboException)
                        {
                            //TODO: show notification
                        }
                    }
                };
            });
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            ImageCache.Instance.InitializeAsync(httpMessageHandler: new WeiboHttpClientHandler());
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {
            if (args is ProtocolActivatedEventArgs protocolActivatedEventArgs)
            {
                EnsureWindow();
                Singleton<BroadcastCenter>.Instance.SendWithPendingMessage(this, "share_target_receive", protocolActivatedEventArgs);
            }
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            EnsureWindow(e.PrelaunchActivated);
        }

        private void EnsureWindow(bool preLaunch = false)
        {
            if (!(Window.Current.Content is RootView))
            {
                Window.Current.Content = new RootView();
            }

            if (preLaunch == false)
            {
                Window.Current.Activate();
            }
        }

        protected override void OnShareTargetActivated(ShareTargetActivatedEventArgs args)
        {
            if (!(Window.Current.Content is ShareTargetView))
            {
                Window.Current.Content = new ShareTargetView(args);
            }
            Window.Current.Activate();
        }
    }
}