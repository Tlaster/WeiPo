using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Mac.Renderer;

internal abstract class RendererObject<TWidget, TControl> : IRenderer<UIView>
    where TWidget : MappingWidget where TControl : class, new()
{
    public void Update(UIView control, MappingWidget widget)
    {
        Update(control as TControl ?? throw new InvalidOperationException() , (TWidget)widget);
    }

    public void AddChild(UIView control, UIView childControl)
    {
        AddChild(control as TControl ?? throw new InvalidOperationException() , childControl);
    }

    public void RemoveChild(UIView control, UIView childControl)
    {
        RemoveChild(control as TControl ?? throw new InvalidOperationException() , childControl);
    }

    public void ReplaceChild(UIView control, int index, UIView newChildControl)
    {
        ReplaceChild(control as TControl ?? throw new InvalidOperationException() , index, newChildControl);
    }

    public virtual UIView? GetChildAt(UIView control, int index)
    {
        return control.Subviews[index];
    }

    public bool IsPanel(UIView value)
    {
        return true;
    }

    UIView IRenderer<UIView>.Create()
    {
        return Create() as UIView ?? throw new InvalidOperationException();
    }

    protected virtual TControl Create()
    {
        return new TControl();
    }

    protected virtual void AddChild(TControl control, UIView childControl)
    {
        switch (control)
        {
            case UIView panel:
                panel.AddSubview(childControl);
                break;
        }
    }

    protected virtual void RemoveChild(TControl control, UIView childControl)
    {
        childControl.RemoveFromSuperview();
    }

    protected virtual void ReplaceChild(TControl control, int index, UIView newChildControl)
    {
        switch (control)
        {
            case UIView panel:
                panel.Subviews[index].RemoveFromSuperview();
                panel.InsertSubview(newChildControl, index);
                break;
        }
    }

    protected abstract void Update(TControl control, TWidget widget);
}