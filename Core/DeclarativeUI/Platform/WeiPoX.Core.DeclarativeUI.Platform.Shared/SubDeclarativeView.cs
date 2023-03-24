using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

#if ANDROID
using Android.Content;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using Control = Android.Views.View;
using UserControl = Android.Widget.FrameLayout;
#elif AVALONIA
using Avalonia.LogicalTree;
using System.Diagnostics;
using Avalonia;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using Control = Avalonia.Controls.Control;
using UserControl = Avalonia.Controls.UserControl;
#elif UIKIT
using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;
using Control = UIKit.UIView;
using UserControl = UIKit.UIViewController;
#elif WINUI3
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using Control = Microsoft.UI.Xaml.UIElement;
using UserControl = Microsoft.UI.Xaml.Controls.UserControl;
#endif


#if ANDROID
namespace WeiPoX.Core.DeclarativeUI.Platform.Android;
#elif AVALONIA
namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia;
#elif UIKIT
namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit;
#elif WINUI3
namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3;
#endif

internal class SubDeclarativeView : AbsDeclarativeControl
{
    private readonly WidgetBuilder _renderer;
    private Control? _renderedControl;
    private Widget? _previousWidget;
    private readonly Func<int, ActualLazyItem> _builder;
#if ANDROID
    public SubDeclarativeView(Context context, Func<int, ActualLazyItem> builder, WidgetBuilder renderer) : base(context)
    {
        _builder = builder;
        _renderer = renderer;
    }
#else
    public SubDeclarativeView(WidgetBuilder renderer, Func<int, ActualLazyItem> builder)
    {
        _renderer = renderer;
        _builder = builder;
    }
#endif
    
    public void SetIndex(int index)
    {
        _ = Render(index);
    }
    
    internal async Task Render(int index)
    {
        var widget = _builder.Invoke(index).Builder.Invoke();
        _renderedControl = await _renderer.BuildIfNeededAsync(_previousWidget, widget, _renderedControl);
        _previousWidget = widget;
        UpdateChild(_renderedControl);
    }
}