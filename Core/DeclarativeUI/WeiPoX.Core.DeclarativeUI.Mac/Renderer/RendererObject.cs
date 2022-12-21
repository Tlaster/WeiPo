using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Mac.Renderer;

internal abstract class RendererObject<TWidget, TControl> : IRenderer<NSView>
    where TWidget : MappingWidget where TControl : class, new()
{
    public void Update(NSView control, MappingWidget widget)
    {
        Update(control as TControl ?? throw new InvalidOperationException(), (TWidget)widget);
    }

    public void AddChild(NSView control, NSView childControl)
    {
        AddChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void RemoveChild(NSView control, NSView childControl)
    {
        RemoveChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void ReplaceChild(NSView control, int index, NSView newChildControl)
    {
        ReplaceChild(control as TControl ?? throw new InvalidOperationException(), index, newChildControl);
    }

    NSView IRenderer<NSView>.Create()
    {
        return Create() as NSView ?? throw new InvalidOperationException();
    }

    protected virtual TControl Create()
    {
        return new TControl();
    }

    protected virtual void AddChild(TControl control, NSView childControl)
    {
        switch (control)
        {
            case NSView panel:
                panel.AddSubview(childControl);
                break;
        }
    }

    protected virtual void RemoveChild(TControl control, NSView childControl)
    {
        childControl.RemoveFromSuperview();
    }

    protected virtual void ReplaceChild(TControl control, int index, NSView newChildControl)
    {
        switch (control)
        {
            case NSView panel:
                panel.ReplaceSubviewWith(panel.Subviews[index], newChildControl);
                break;
        }
    }

    protected abstract void Update(TControl control, TWidget widget);
}