using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WeiPo.Common;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo
{
    public sealed partial class ShareTargetView
    {
        private readonly ShareTargetActivatedEventArgs _shareArguments;

        public ShareTargetView(ShareTargetActivatedEventArgs args)
        {
            this.InitializeComponent();
            _shareArguments = args;
            LaunchApp();
        }

        private async void LaunchApp()
        {
            var testAppUri = new Uri("weipo:"); // The protocol handled by the launched app
            var options = new LauncherOptions {TargetApplicationPackageFamilyName = Package.Current.Id.FamilyName};

            var inputData = new ValueSet();
            var data = _shareArguments.ShareOperation.Data;
            if (data.Contains(StandardDataFormats.Text))
            {
                var text = await data.GetTextAsync();
                inputData["text"] = text;
            }

            if (data.Contains(StandardDataFormats.Bitmap))
            {
                var bitmap = await data.GetBitmapAsync();
                var file = await bitmap.SaveCacheFile();
                inputData["image"] = SharedStorageAccessManager.AddFile(file);
            }

            await Launcher.LaunchUriAsync(testAppUri, options, inputData);
            _shareArguments.ShareOperation.ReportCompleted();
        }
    }
}
