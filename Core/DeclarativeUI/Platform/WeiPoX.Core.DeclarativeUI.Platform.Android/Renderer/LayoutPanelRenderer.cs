using System.Collections.Immutable;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets.Layout;
using Rect = WeiPoX.Core.DeclarativeUI.Foundation.Rect;
using Size = WeiPoX.Core.DeclarativeUI.Foundation.Size;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

public abstract class LayoutPanelRenderer<T> : RendererObject<T, WeiPoXPanel> where T: LayoutPanel
{
    protected override void Update(WeiPoXPanel control, T widget)
    {
        control.Panel = widget;
    }

    protected LayoutPanelRenderer(Context context) : base(context)
    {
    }

    protected override WeiPoXPanel Create(Context context, RendererContext<View> rendererContext)
    {
        return new WeiPoXPanel(context);
    }
}

public class WeiPoXPanel : ViewGroup
{
    private ILayoutPanel? _panel;

    public ILayoutPanel? Panel
    {
        get => _panel;
        set
        {
            if (value != null && value != _panel)
            {
                _panel = value;
                Invalidate();
            }
            else
            {
                _panel = value;
            }
        }
    }

    private ImmutableList<ILayoutChild> GetChildren()
    {
        var builder = ImmutableList.CreateBuilder<ILayoutChild>();
        for (var i = 0; i < ChildCount; i++)
        {
            var child = GetChildAt(i);
            if (child != null)
            {
                builder.Add(new LayoutChild(child));
            }
        }

        return builder.ToImmutable();
    }

    public WeiPoXPanel(Context context) : base(context)
    {
    }

    protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
    {
        if (_panel == null || ChildCount == 0)
        {
            base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
        }
        else
        {
            var size = _panel.Measure(new LayoutContext(
                new Size(MeasureSpec.GetSize(widthMeasureSpec), MeasureSpec.GetSize(heightMeasureSpec)),
                GetChildren()));
            // keep consistent with WinUI/Avalonia/MAUI
            var isRecyclerViewRoot = Parent is DeclarativeView && Parent.Parent is RecyclerView;
            SetMeasuredDimension(isRecyclerViewRoot ? MeasureSpec.GetSize(widthMeasureSpec) :(int) size.Width, (int) size.Height);
        }
    }

    protected override void OnLayout(bool changed, int l, int t, int r, int b)
    {
        if (_panel != null && ChildCount != 0)
        {
            _panel.Arrange(new LayoutContext(new Size(r - l, b - t), GetChildren()));
        }
    }
}

internal class LayoutChild : ILayoutChild
{
    private readonly View _element;

    public LayoutChild(View element)
    {
        _element = element;
    }

    public Foundation.Size Measure(Foundation.Size availableSize)
    {
        _element.Measure(View.MeasureSpec.MakeMeasureSpec((int) availableSize.Width, MeasureSpecMode.Unspecified),
            View.MeasureSpec.MakeMeasureSpec((int) availableSize.Height, MeasureSpecMode.Unspecified));
        return new Size(_element.MeasuredWidth, _element.MeasuredHeight);
    }

    public void Arrange(Rect rect)
    {
        _element.Layout((int) rect.X, (int) rect.Y, (int) (rect.X + rect.Width), (int) (rect.Y + rect.Height));
    }

    public Size DesiredSize => new Size(_element.MeasuredWidth, _element.MeasuredHeight);
}

internal class LayoutContext : ILayoutContext
{
    public LayoutContext(Size size, ImmutableList<ILayoutChild> children)
    {
        Children = children;
        Size = size;
    }

    public Size Size { get; }
    public ImmutableList<ILayoutChild> Children { get; }
    public double ToNativePixels(double value)
    {
        return value.ToDp();
    }
}