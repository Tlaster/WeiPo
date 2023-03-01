using Android.OS;
using AndroidX.Activity;
using WeiPoX.Core.DeclarativeUI.Platform.Android;
using WeiPoX.Core.DeclarativeUI.Sample.Core;

namespace WeiPoX.Core.DeclarativeUI.Sample.Android;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : ComponentActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(new Declarative(this, new SampleApp()));
    }
}