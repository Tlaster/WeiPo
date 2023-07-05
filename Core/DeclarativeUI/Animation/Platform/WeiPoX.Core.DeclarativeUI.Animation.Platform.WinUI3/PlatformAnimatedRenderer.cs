using System;
using System.Numerics;
using Microsoft.UI.Xaml;
using WeiPoX.Core.DeclarativeUI.Foundation;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Animation.Platform.WinUI3;

public class PlatformAnimatedRenderer : BoxRenderer
{
    protected override WeiPoXPanel Create(RendererContext<UIElement> context)
    {
        return new WeiPoXAnimatedPanel();
    }

    protected override void Update(WeiPoXPanel control, Box widget)
    {
        base.Update(control, widget);
        if (widget is not PlatformAnimated platformAnimated || control is not WeiPoXAnimatedPanel animatedPanel)
        {
            return;
        }

        if (platformAnimated is { Initial: not null, Target: not null })
        {
            animatedPanel.SetTransitionData(platformAnimated.Initial, platformAnimated.Target, platformAnimated.Duration);
        }
        animatedPanel.SetDuration(platformAnimated.Duration);
    }
}

internal class WeiPoXAnimatedPanel : WeiPoXPanel
{
    private TransitionData? _initial;
    private TransitionData? _target;
    public void SetTransitionData(TransitionData initial, TransitionData target, TimeSpan duration)
    {
        if (initial != _initial || target != _target)
        {
            if (_initial == null && _target == null)
            {
                SetInitialState(target);
            }
            else
            {
                PlayTransition(target);
            }
        }
        _initial = initial;
        _target = target;
    }

    private void SetInitialState(TransitionData target)
    {
        PlayTransition(target);
        OpacityTransition = new ScalarTransition();
        RotationTransition = new ScalarTransition();
        ScaleTransition = new Vector3Transition();
        TranslationTransition = new Vector3Transition();
    }

    private void PlayTransition(TransitionData target)
    {
        Opacity = target.Fade?.Alpha ?? 1f;
        // TODO: not working
        // RenderTransformOrigin = new Point(target.Scale?.OriginX ?? 0.5, target.Scale?.OriginY ?? 0.5);
        // TODO: initial height/width is 0
        var slideOffset = target.Slide?.SlideOffset.Invoke(new Offset(ActualWidth, ActualHeight)) ?? new Offset(0f, 0f);
        Translation = new Vector3((float) slideOffset.X, (float) slideOffset.Y, 0f);
        Scale = new Vector3(target.Scale?.ScaleX ?? 1f, target.Scale?.ScaleY ?? 1f, 1f);

    }

    public void SetDuration(TimeSpan platformAnimatedDuration)
    {
        OpacityTransition.Duration = platformAnimatedDuration;
        RotationTransition.Duration = platformAnimatedDuration;
        ScaleTransition.Duration = platformAnimatedDuration;
        TranslationTransition.Duration = platformAnimatedDuration;
    }
}