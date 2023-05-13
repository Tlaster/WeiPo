using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using WeiPoX.Core.DeclarativeUI.Foundation;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;

namespace WeiPoX.Core.DeclarativeUI.Animation.Platform.Avalonia;

public class PlatformAnimatedRenderer : BoxRenderer
{
    protected override WeiPoXPanel Create(RendererContext<Control> context)
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
            animatedPanel.SetTransitionData(platformAnimated.Initial, platformAnimated.Target);
        }
        animatedPanel.SetDuration(platformAnimated.Duration);
    }
}

internal class WeiPoXAnimatedPanel : WeiPoXPanel
{
    public WeiPoXAnimatedPanel()
    {
        Transitions = new global::Avalonia.Animation.Transitions
        {
            new DoubleTransition
            {
                Property = OpacityProperty,
            },
            new TransformOperationsTransition
            {
                Property = RenderTransformProperty,
            }
        };
    }

    private TransitionData? _initial;
    private TransitionData? _target;
    public void SetTransitionData(TransitionData initial, TransitionData target)
    {
        if (initial != _initial || target != _target)
        {
            PlayTransition(initial, target);
        }
        _initial = initial;
        _target = target;
    }

    private void PlayTransition(TransitionData initial, TransitionData target)
    {
        Opacity = target.Fade?.Alpha ?? 0f;
        RenderTransformOrigin = new RelativePoint(target.Scale?.OriginX ?? 0.5, target.Scale?.OriginY ?? 0.5,
            RelativeUnit.Relative);
        var slideOffset = target.Slide?.SlideOffset.Invoke(new Offset(Width, Height)) ?? new Offset(0f, 0f);
        RenderTransform = new MatrixTransform(
            new Matrix(
                scaleX: target.Scale?.ScaleX ?? 1f,
                scaleY: target.Scale?.ScaleY ?? 1f,
                skewY: 0.0,
                skewX: 0.0,
                offsetX: slideOffset.X,
                offsetY: slideOffset.Y
            )
        );
    }

    public void SetDuration(TimeSpan platformAnimatedDuration)
    {
        if (Transitions == null)
        {
            return;
        }
        foreach (var transition in Transitions)
        {
            switch (transition)
            {
                case DoubleTransition doubleTransition:
                    doubleTransition.Duration = platformAnimatedDuration;
                    break;
                case TransformOperationsTransition transformOperationsTransition:
                    transformOperationsTransition.Duration = platformAnimatedDuration;
                    break;
            }
        }
    }
}