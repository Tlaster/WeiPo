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



public class DeclarativeView : UserControl, IBuildOwner
{
    private List<Widget> RebuiltWidgets { get; } = new();
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
        Render();
    }
#else 
    public DeclarativeView(Widget widget)
    {
        _widget = widget;
        _renderer = new WidgetBuilder(this);
        Render();
    }
#endif


    public void MarkNeedsBuild(Widget widget)
    {
        RebuiltWidgets.Add(widget);
        _requestBuildCount++;
        if (!_rendering)
        {
            Render();
        }
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return RebuiltWidgets.Contains(widget);
    }

    public void CleanUp()
    {
        RebuiltWidgets.Clear();
    }

    private async void Render()
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
            }
            else
            {
                _requestBuildCount = 1;
            }
        }
    }

    private void UpdateChild(Control control)
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
            ApplySafeArea();
        }
        else if (!View.Subviews[0].Equals(control))
        {
            View.Subviews[0].RemoveFromSuperview();
            View.AddSubview(control);
            ApplySafeArea();
        }
#elif WINUI3
        if (!Equals(Content, control))
        {
            Content = control;
        }
#endif
    }
    
#if UIKIT
    private void ApplySafeArea()
    {
        if (_renderedControl == null)
        {
            return;
        }
        _renderedControl.TranslatesAutoresizingMaskIntoConstraints = false;
        var guide = View?.LayoutMarginsGuide;
        if (guide != null)
        {
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _renderedControl.LeadingAnchor.ConstraintEqualTo(guide.LeadingAnchor),
                _renderedControl.TrailingAnchor.ConstraintEqualTo(guide.TrailingAnchor),
            });
        }
        var safeGuide = View?.SafeAreaLayoutGuide;
        if (safeGuide != null)
        {
            NSLayoutConstraint.ActivateConstraints(new[]
            {
                _renderedControl.TopAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.TopAnchor, 1),
                _renderedControl.BottomAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.BottomAnchor, 1),
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