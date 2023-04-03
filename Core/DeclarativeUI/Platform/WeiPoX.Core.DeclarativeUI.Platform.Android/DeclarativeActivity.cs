using AndroidX.Core.App;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android;

public abstract class DeclarativeActivity<T> : ComponentActivity where T : Widget, new() {
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(new DeclarativeView(this)
        {
            Widget = new T()
        });
    }
}