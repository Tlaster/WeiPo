using System.Collections.Generic;
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

public abstract class AbsDeclarativeControl : UserControl
{
    
#if ANDROID
    protected AbsDeclarativeControl(Context context) : base(context)
    {
    }
#else
    protected AbsDeclarativeControl()
    {
    }
#endif
    
    internal void UpdateChild(Control control)
    {
#if ANDROID
        if (ChildCount == 0)
        {
            AddView(control);
        }
        else if (control != GetChildAt(0))
        {
            RemoveViewAt(0);
            AddView(control);
        }
#elif AVALONIA
        if (!Equals(Content, control))
        {
            Content = control;
        }
#elif UIKIT
        if (View == null)
        {
            return;
        }
        if (View.Subviews.Length == 0)
        {
            View.AddSubview(control);
            ApplySafeArea(control);
        }
        else if (!View.Subviews[0].Equals(control))
        {
            View.Subviews[0].RemoveFromSuperview();
            View.AddSubview(control);
            ApplySafeArea(control);
        }
#elif WINUI3
        if (!Equals(Content, control))
        {
            Content = control;
        }
#endif
    }
    
#if UIKIT
    private void ApplySafeArea(Control control)
    {
        control.TranslatesAutoresizingMaskIntoConstraints = false;
        var guide = View?.LayoutMarginsGuide;
        if (guide != null)
        {
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                control.LeadingAnchor.ConstraintEqualTo(guide.LeadingAnchor),
                control.TrailingAnchor.ConstraintEqualTo(guide.TrailingAnchor),
            });
        }
        var safeGuide = View?.SafeAreaLayoutGuide;
        if (safeGuide != null)
        {
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                control.TopAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.TopAnchor, 1),
                control.BottomAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.BottomAnchor, 1),
            });
        }
    }
    
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();
        if (View != null)
        {
            View.BackgroundColor = UIColor.SystemBackground;
        }
    }
#endif
}