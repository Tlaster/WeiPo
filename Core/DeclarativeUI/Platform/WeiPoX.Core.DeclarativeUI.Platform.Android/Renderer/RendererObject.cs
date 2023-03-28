using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

public abstract class RendererObject<TWidget, TControl> : IRenderer<View>
    where TWidget : MappingWidget where TControl : class
{
    private readonly Context _context;

    protected RendererObject(Context context)
    {
        _context = context;
    }

    public void Update(View control, MappingWidget widget)
    {
        Update(control as TControl ?? throw new InvalidOperationException(), (TWidget)widget);
    }

    public void AddChild(View control, View childControl)
    {
        AddChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void RemoveChild(View control, View childControl)
    {
        RemoveChild(control as TControl ?? throw new InvalidOperationException(), childControl);
    }

    public void ReplaceChild(View control, int index, View newChildControl)
    {
        ReplaceChild(control as TControl ?? throw new InvalidOperationException(), index, newChildControl);
    }
    
    public bool IsPanel(View value)
    {
        return value is ViewGroup;
    }

    public virtual View? GetChildAt(View control, int index)
    {
        if (control is ViewGroup panel)
        {
            return panel.GetChildAt(index);
        }

        return null;
    }

    View IRenderer<View>.Create(RendererContext<View> context)
    {
        return Create(_context, context ?? throw new InvalidOperationException()) as View ??
               throw new InvalidOperationException();
    }

    protected abstract TControl Create(Context context, RendererContext<View> rendererContext);

    protected virtual void AddChild(TControl control, View childControl)
    {
        switch (control)
        {
            case ViewGroup group:
                group.AddView(childControl);
                break;
        }
    }

    protected virtual void RemoveChild(TControl control, View childControl)
    {
        switch (control)
        {
            case ViewGroup group:
                group.RemoveView(childControl);
                break;
        }
    }

    protected virtual void ReplaceChild(TControl control, int index, View newChildControl)
    {
        switch (control)
        {
            case ViewGroup group:
                group.RemoveViewAt(index);
                group.AddView(newChildControl, index);
                break;
        }
    }

    protected abstract void Update(TControl control, TWidget widget);
}

public abstract class LazyRendererObject<TWidget, TControl> : RendererObject<TWidget, TControl>, ILazyRenderer<View>
    where TWidget : MappingWidget where TControl : class
{
    public bool IsVisible(View control, int index)
    {
        return IsVisible(control as TControl ?? throw new InvalidOperationException(), index);
    }
    
    public View? GetVisibleChild(View control, int index)
    {
        return GetVisibleChild(control as TControl ?? throw new InvalidOperationException(), index);
    }

    public void UpdateChild(View control, int index, View childControl)
    {
        UpdateChild(control as TControl ?? throw new InvalidOperationException(), index, childControl);
    }

    protected abstract bool IsVisible(TControl control, int index);

    protected abstract View? GetVisibleChild(TControl control, int index);

    protected abstract void UpdateChild(TControl control, int index, View childControl);

    protected LazyRendererObject(Context context) : base(context)
    {
    }
}