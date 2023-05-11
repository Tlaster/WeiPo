using Avalonia.Animation;
using Avalonia.Controls;
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
        if (widget is PlatformAnimated platformAnimated && control is WeiPoXAnimatedPanel animatedPanel)
        {
            
        }
    }
}

internal class WeiPoXAnimatedPanel : WeiPoXPanel
{
    public WeiPoXAnimatedPanel()
    {
        this.Transitions?.Add(new DoubleTransition
        {
            Property = OpacityProperty,
        });
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
        this.Opacity = target.Fade?.Alpha ?? 0f;
    }
}