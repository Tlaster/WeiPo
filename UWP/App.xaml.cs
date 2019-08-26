using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using WeiPo.Common;

namespace WeiPo
{
    sealed partial class App
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            FlurlHttp.Configure(settings => {
                var jsonSettings = new JsonSerializerSettings
                {
                    Converters =
                    {
                        new JsonNumberConverter(),
                    },
                    NullValueHandling = NullValueHandling.Ignore,
                };
                
                settings.JsonSerializer = new WeiboJsonSerializer(jsonSettings);
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
