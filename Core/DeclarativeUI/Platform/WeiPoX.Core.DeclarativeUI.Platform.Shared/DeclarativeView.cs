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



public class DeclarativeView : AbsDeclarativeControl, IBuildOwner
{
    private readonly List<Widget> _rebuiltWidgets = new();
    private readonly WidgetBuilder _renderer;
    private Control? _renderedControl;
    private bool _rendering;
    private int _requestBuildCount = 1;
    private readonly Widget _widget;

#if ANDROID
    public DeclarativeView(Context context, Widget widget) : base(context)
    {
        _widget = widget;
        _renderer = new WidgetBuilder(this, context);
        _ = Render();
    }
#else 
    public DeclarativeView(Widget widget)
    {
        _widget = widget;
        _renderer = new WidgetBuilder(this);
        _ = Render();
    }
#endif
    
    public void MarkNeedsBuild(Widget widget)
    {
        _rebuiltWidgets.Add(widget);
        _requestBuildCount++;
        if (!_rendering)
        {
            _ = Render();
        }
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return _rebuiltWidgets.Contains(widget);
    }

    public void CleanUp()
    {
        _rebuiltWidgets.Clear();
    }

    private async Task Render()
    {
        while (_requestBuildCount > 0)
        {
            _rendering = true;
            _renderedControl = await _renderer.BuildIfNeededAsync(_widget, _widget, _renderedControl);
            _rendering = false;
            _requestBuildCount--;
            if (_requestBuildCount == 0)
            {
                UpdateChild(_renderedControl);
                CleanUp();
            }
            else
            {
                _requestBuildCount = 1;
            }
        }
    }
}
