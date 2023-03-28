using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.UIKit.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

public abstract class RendererObject<TWidget, TControl> : IRenderer<UIView>
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

    UIView IRenderer<UIView>.Create(RendererContext<UIView> context)
    {
        return Create(context ?? throw new InvalidOperationException()) as UIView ?? throw new InvalidOperationException();
    }

    protected virtual TControl Create(RendererContext<UIView> context)
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


public abstract class LazyRendererObject<TWidget, TControl> : RendererObject<TWidget, TControl>, ILazyRenderer<UIView>
    where TWidget : MappingWidget where TControl : class, new()
{
    public bool IsVisible(UIView control, int index)
    {
        return IsVisible(control as TControl ?? throw new InvalidOperationException(), index);
    }

    public UIView? GetVisibleChild(UIView control, int index)
    {
        return GetVisibleChild(control as TControl ?? throw new InvalidOperationException(), index);
    }

    public void UpdateChild(UIView control, int index, UIView childControl)
    {
        UpdateChild(control as TControl ?? throw new InvalidOperationException(), index, childControl);
    }

    protected abstract bool IsVisible(TControl control, int index);

    protected abstract UIView? GetVisibleChild(TControl control, int index);

    protected abstract void UpdateChild(TControl control, int index, UIView childControl);
}