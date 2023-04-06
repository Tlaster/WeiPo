using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI;

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
    private Func<Widget>? _previousWidgetBuilder;
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
            view.Update((RepeaterItem)newvalue);
        }
    }

    internal RepeaterItem RepeaterItem
    {
        get => (RepeaterItem)GetValue(RepeaterItemProperty);
        set => SetValue(RepeaterItemProperty, value);
    }
    

    private void Update(RepeaterItem item)
    {
        if (_previousBuildOwner != item.BuildOwner)
        {
            _core = new DeclarativeCore<View>(new WidgetBuilder(), UpdateChild, item.BuildOwner);
        }
        _previousBuildOwner = item.BuildOwner;
        if (_previousWidgetBuilder != item.WidgetBuilder && _core != null)
        {
            _core.Widget = item.WidgetBuilder();
        }
        _previousWidgetBuilder = item.WidgetBuilder;
    }
    
    internal void UpdateChild(View control)
    {
        if (!Equals(Content, control))
        {
            Content = control;
        }
    }
}