using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using AndroidX.Activity;
using AndroidX.Lifecycle;
using WeiPoX.Core.DeclarativeUI.Foundation;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;
using Object = Java.Lang.Object;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android;

public abstract class DeclarativeActivity<T> : ComponentActivity where T : Widget, new() 
{
    private DeclarativeViewModel? _viewModel;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        _viewModel = new ViewModelProvider(this).Get(Java.Lang.Class.FromType(typeof(DeclarativeViewModel))) as DeclarativeViewModel;
        OnBackPressedDispatcher.AddCallback(this, _viewModel!.BackPressedCallback);
        SetContentView(new DeclarativeView(this)
        {
            Widget = new AppWidget
            {
                App = new T(),
                AppState = _viewModel!.AppState
            }
        });
    }

    protected override void OnResume()
    {
        base.OnResume();
        if (_viewModel?.AppState.LifecycleHolder != null)
        {
            _viewModel.AppState.LifecycleHolder.CurrentLifecycleState = LifecycleState.Active;
        }
    }

    protected override void OnPause()
    {
        base.OnPause();
        if (_viewModel?.AppState.LifecycleHolder != null)
        {
            _viewModel.AppState.LifecycleHolder.CurrentLifecycleState = LifecycleState.InActive;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_viewModel?.AppState.LifecycleHolder != null && !IsChangingConfigurations)
        {
            _viewModel.AppState.LifecycleHolder.CurrentLifecycleState = LifecycleState.Destroyed;
        }
    }
}

internal class DeclarativeViewModel : ViewModel
{
    private readonly IDisposable _disposable;

    public DeclarativeViewModel()
    {
        BackPressedCallback = new BackPressedCallback(true, () => AppState.BackDispatcher.OnBackPressed());
        _disposable = AppState.BackDispatcher.CanGoBack.Subscribe(canGoBack => BackPressedCallback.Enabled = canGoBack);
    }

    public AppState AppState { get; } = new();
    public BackPressedCallback BackPressedCallback { get; }

    protected override void OnCleared()
    {
        base.OnCleared();
        _disposable.Dispose();
    }
}

internal class BackPressedCallback : OnBackPressedCallback
{
    private readonly Action _onBackPressed;

    public BackPressedCallback(bool enabled, Action onBackPressed) : base(enabled)
    {
        _onBackPressed = onBackPressed;
    }

    public override void HandleOnBackPressed()
    {
        _onBackPressed();
    }
}
