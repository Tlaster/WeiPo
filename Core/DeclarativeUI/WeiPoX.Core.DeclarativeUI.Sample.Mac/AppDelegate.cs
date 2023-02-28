using WeiPoX.Core.DeclarativeUI.Platform.Mac;
using WeiPoX.Core.DeclarativeUI.Sample.Core;

namespace WeiPoX.Core.DeclarativeUI.Sample.Mac;

[Register("AppDelegate")]
public class AppDelegate : NSApplicationDelegate
{
    public AppDelegate()
    {
        window = new NSWindow(new CGRect(0.0, 0.0, 640.0, 480.0),
            NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable,
            NSBackingStore.Buffered, false)
        {
            ContentView = new Declarative(new App())
        };
        window.Center();
        window.OrderFrontRegardless();
    }

    public NSWindow? window { get; set; }

    public override void WillTerminate(NSNotification notification)
    {
        // Insert code here to tear down your application
    }
}