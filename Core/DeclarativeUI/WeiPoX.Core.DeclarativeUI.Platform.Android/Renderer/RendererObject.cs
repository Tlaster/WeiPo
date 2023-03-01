using Android.Content;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal abstract class RendererObject<TWidget, TControl> : IRenderer<View>
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

    View IRenderer<View>.Create()
    {
        return Create(_context) as View ?? throw new InvalidOperationException();
    }

    protected abstract TControl Create(Context context);

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