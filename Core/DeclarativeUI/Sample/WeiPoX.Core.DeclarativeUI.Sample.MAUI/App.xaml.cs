using WeiPoX.Core.DeclarativeUI.Platform.MAUI;
using WeiPoX.Core.DeclarativeUI.Sample.Core;

namespace WeiPoX.Core.DeclarativeUI.Sample.MAUI;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new DeclarativePage<SampleApp>();
    }
}