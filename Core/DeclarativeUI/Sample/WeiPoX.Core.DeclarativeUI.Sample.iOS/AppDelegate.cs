using WeiPoX.Core.DeclarativeUI.Platform.UIKit;
using WeiPoX.Core.DeclarativeUI.Sample.Core;

namespace WeiPoX.Core.DeclarativeUI.Sample.iOS;

[Register("AppDelegate")]
public class AppDelegate : UIApplicationDelegate
{
    public override UIWindow? Window { get; set; }

    public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
    {
        Window = new UIWindow(UIScreen.MainScreen.Bounds);
        Window.RootViewController = new UIViewController
        {
            View = new DeclarativeView
            {
                Widget = new SampleApp()
            }
        };
        Window.MakeKeyAndVisible();
        return true;
    }
}