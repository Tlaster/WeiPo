﻿using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit;

public class DeclarativeView : UIView
{
    private DeclarativeCore<UIView>? _core;
    
    internal void Init(IBuildOwner? buildOwner = null)
    {
        _core ??= new DeclarativeCore<UIView>(new WidgetBuilder(), UpdateChild, buildOwner);
    }
    
    public Widget? Widget
    {
        get => _core?.Widget;
        set
        {
            if (_core != null)
            {
                _core.Widget = value;
            }
        }
    }

    internal void UpdateChild(UIView control)
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
    
    private void ApplySafeArea(UIView control)
    {
        control.TranslatesAutoresizingMaskIntoConstraints = false;
        var guide = LayoutMarginsGuide;
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            control.LeadingAnchor.ConstraintEqualTo(guide.LeadingAnchor),
            control.TrailingAnchor.ConstraintEqualTo(guide.TrailingAnchor),
        });
        var safeGuide = SafeAreaLayoutGuide;
        NSLayoutConstraint.ActivateConstraints(new[]
        {
            control.TopAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.TopAnchor, 1),
            control.BottomAnchor.ConstraintEqualToSystemSpacingBelowAnchor(safeGuide.BottomAnchor, 1),
        });
    }
}