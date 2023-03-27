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
using UserControl = UIKit.UIView;
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

internal class SubDeclarativeView :
#if UIKIT
    UserControl
#else
    AbsDeclarativeControl
#endif
{
    private Control? _renderedControl;
    private Widget? _previousWidget;
    public Func<int, ActualLazyItem>? Builder
    {
        get;
#if UIKIT
        set;
#endif
    }

    public WidgetBuilder? Renderer
    {
        get;
#if UIKIT
        set;
#endif
    }
#if ANDROID
    public SubDeclarativeView(Context context, Func<int, ActualLazyItem> builder, WidgetBuilder renderer) : base(context)
    {
        Renderer = renderer;
        Builder = builder;
    }
#elif !UIKIT
    public SubDeclarativeView(WidgetBuilder renderer, Func<int, ActualLazyItem> builder)
    {
        Renderer = renderer;
        Builder = builder;
    }
#endif
    
    public void SetIndex(int index)
    {
        _ = Render(index);
    }

    private async Task Render(int index)
    {
#if UIKIT
        if (Builder == null || Renderer == null)
        {
            return;
        }
#endif
        var widget = Builder.Invoke(index).Builder.Invoke();
        _renderedControl = await Renderer.BuildIfNeededAsync(_previousWidget, widget, _renderedControl);
        _previousWidget = widget;
        UpdateChild(_renderedControl);
    }

#if UIKIT
    internal void UpdateChild(Control control)
    {
        if (Subviews.Length == 0)
        {
            AddSubview(control);
            ApplySafeArea(control);
        }
        else if (!Subviews[0].Equals(control))
        {
            Subviews[0].RemoveFromSuperview();
            AddSubview(control);
            ApplySafeArea(control);
        }
    }
    private void ApplySafeArea(Control control)
    {
        control.TranslatesAutoresizingMaskIntoConstraints = false;
        var guide = LayoutMarginsGuide;
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            control.LeadingAnchor.ConstraintEqualTo(guide.LeadingAnchor),
            control.TrailingAnchor.ConstraintEqualTo(guide.TrailingAnchor),
        });
        if (SafeAreaLayoutGuide is { } safeGuide)
        {
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                control.TopAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.TopAnchor, 1),
                control.BottomAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.BottomAnchor, 1),
            });
        }
    }
#endif
}