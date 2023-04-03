using Android.OS;
using AndroidX.Activity;
using WeiPoX.Core.DeclarativeUI.Platform.Android;
using WeiPoX.Core.DeclarativeUI.Sample.Core;

namespace WeiPoX.Core.DeclarativeUI.Sample.Android;

[Activity(Label = "@string/app_name", MainLauncher = true)]
public class MainActivity : DeclarativeActivity<SampleApp>
{
}