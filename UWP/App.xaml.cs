using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Flurl.Http;
using Newtonsoft.Json;
using WeiPo.Common;

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
                    NullValueHandling = NullValueHandling.Ignore
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