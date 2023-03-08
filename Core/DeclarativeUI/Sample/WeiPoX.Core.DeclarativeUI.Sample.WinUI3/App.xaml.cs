using WeiPoX.Core.DeclarativeUI.Platform.WinUI3;
using WeiPoX.Core.DeclarativeUI.Sample.Core;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Sample.WinUI3
{
    public partial class App : WeiPoXDeclarativeUiApp
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override Widget CreateWidget()
        {
            return new SampleApp();
        }
    }
}
