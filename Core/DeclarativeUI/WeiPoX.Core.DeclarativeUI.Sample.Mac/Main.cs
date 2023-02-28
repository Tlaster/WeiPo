using WeiPoX.Core.DeclarativeUI.Sample.Mac;

// This is the main entry point of the application.
NSApplication.Init();
NSApplication.SharedApplication.Delegate = new AppDelegate();
NSApplication.Main(args);
// var style = NSWindowStyle.Titled | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Resizable;
// var rect = new CGRect(0.0, 0.0, 640.0, 480.0);
// var window = new NSWindow(rect, style, NSBackingStore.Buffered, true)
// {
//     Title = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleDisplayName").ToString(),
//     TitleVisibility = NSWindowTitleVisibility.Visible,
//     MinSize = rect.Size,
//     FrameAutosaveName = NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleDisplayName").ToString(),
// };
//
// window.Center();
// window.OrderFrontRegardless();
// NSApplication.SharedApplication.Run();

