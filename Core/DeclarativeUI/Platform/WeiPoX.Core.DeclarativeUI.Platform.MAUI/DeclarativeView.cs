using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI;

public class DeclarativePage<T> : ContentPage where T: Widget, new()
{
    private readonly AppState _appState = new();
    public DeclarativePage()
    {
        Content = new DeclarativeView
        {
            Widget = new AppWidget
            {
                App = new T(),
                AppState = _appState,
            }
        };
    }

    protected override bool OnBackButtonPressed()
    {
        return _appState.BackDispatcher.OnBackPressed();
    }
}

public class DeclarativeView : ContentView
{
    private readonly DeclarativeCore<View> _core;

    public DeclarativeView(IBuildOwner? buildOwner = null)
    {
        _core = new DeclarativeCore<View>(new WidgetBuilder(), UpdateChild, buildOwner);
    }

    public Widget? Widget
    {
        get => _core.Widget;
        set => _core.Widget = value;
    }

    internal void UpdateChild(View control)
    {
        if (!Equals(Content, control))
        {
            Content = control;
        }
    }
}

public class RepeaterDeclarativeView : ContentView
{
    private IBuildOwner? _previousBuildOwner;
    private Func<Widget?>? _previousWidgetBuilder;
    private DeclarativeCore<View>? _core;

    public static DataTemplate GenerateDataTemplate()
    {
        return new DataTemplate(() =>
        {
            var view = new RepeaterDeclarativeView();
            view.SetBinding(RepeaterItemProperty, ".");
            return view;
        });
    }

    public static readonly BindableProperty RepeaterItemProperty = BindableProperty.Create(
        nameof(RepeaterItem), typeof(RepeaterItem), typeof(RepeaterDeclarativeView), propertyChanged: OnPropertyChanged);

    private static void OnPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
    {
        if (bindable is RepeaterDeclarativeView view)
        {
            view.Update((RepeaterItem?)oldvalue, (RepeaterItem)newvalue);
        }
    }

    internal RepeaterItem RepeaterItem
    {
        get => (RepeaterItem)GetValue(RepeaterItemProperty);
        set => SetValue(RepeaterItemProperty, value);
    }

    public RepeaterDeclarativeView()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    private void OnUnloaded(object? sender, EventArgs e)
    {
        RepeaterItem?.UnLoaded(this);
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        RepeaterItem?.Loaded(this);
    }


    private void Update(RepeaterItem? oldItem, RepeaterItem newItem)
    {
        if (_previousBuildOwner != newItem.BuildOwner)
        {
            _core = new DeclarativeCore<View>(new WidgetBuilder(), UpdateChild, newItem.BuildOwner);
        }
        _previousBuildOwner = newItem.BuildOwner;
        if (_previousWidgetBuilder != newItem.WidgetBuilder && _core != null)
        {
            _core.Widget = newItem.WidgetBuilder();
        }
        _previousWidgetBuilder = newItem.WidgetBuilder;
    }
    
    internal void UpdateChild(View control)
    {
        if (!Equals(Content, control))
        {
            Content = control;
        }
    }
}