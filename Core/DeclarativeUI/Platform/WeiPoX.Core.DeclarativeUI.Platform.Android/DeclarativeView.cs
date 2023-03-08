using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android;

public abstract class DeclarativeView : FrameLayout, IBuildOwner
{
    public List<Widget> RebuiltWidgets { get; } = new();
    private readonly WidgetBuilder _renderer;
    private View? _content;
    private bool _rendering;
    private bool _requireReRender;

    public void MarkNeedsBuild(Widget widget)
    {
        RebuiltWidgets.Add(widget);
        if (_rendering)
        {
            _requireReRender = true;
        }
        else
        {
            Render();
        }
    }

    public bool IsBuildScheduled(Widget widget)
    {
        return RebuiltWidgets.Contains(widget);
    }

    public void CleanUp()
    {
        RebuiltWidgets.Clear();
    }

    protected void Render()
    {
        _rendering = true;
        _content = _renderer.BuildIfNeeded(Widget, Widget, _content);
        _rendering = false;
        if (!_requireReRender)
        {
            if (ChildCount == 0)
            {
                AddView(_content);
            }
            else if (_content != GetChildAt(0))
            {
                RemoveViewAt(0);
                AddView(_content);
            }
            return;
        }

        _requireReRender = false;
        Render();
    }

    protected abstract Widget Widget { get; }

    protected DeclarativeView(Context context) : base(context)
    {
        _renderer = new WidgetBuilder(this, context);
    }

    protected DeclarativeView(Context context, IAttributeSet? attrs) : base(context, attrs)
    {
        _renderer = new WidgetBuilder(this, context);
    }

    protected DeclarativeView(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
    {
        _renderer = new WidgetBuilder(this, context);
    }

    protected DeclarativeView(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
    {
        _renderer = new WidgetBuilder(this, context);
    }

    // protected override void OnLayout(bool changed, int l, int t, int r, int b)
    // {
    //     _content?.Layout(l, t, r, b);
    // }
    //
    // protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
    // {
    //     _content?.Measure(widthMeasureSpec: widthMeasureSpec, heightMeasureSpec: heightMeasureSpec);
    //     SetMeasuredDimension(widthMeasureSpec, heightMeasureSpec);
    // }
    //
    // protected override void OnDraw(Canvas? canvas)
    // {
    //     _content?.Draw(canvas);
    // }
    //
    // public override bool OnTouchEvent(MotionEvent? e)
    // {
    //     return _content?.OnTouchEvent(e) ?? base.OnTouchEvent(e);
    // }
    //
    // public override bool DispatchTouchEvent(MotionEvent? e)
    // {
    //     return _content?.DispatchTouchEvent(e) ?? base.DispatchTouchEvent(e);
    // }

    // public override int ChildCount => _content == null ? base.ChildCount : 1;
    // public override View? GetChildAt(int index)
    // {
    //     return _content ?? base.GetChildAt(index);
    // }
}
public class Declarative : DeclarativeView
{
    protected override Widget Widget { get; }

    public Declarative(Context context, Widget widget) : base(context)
    {
        Widget = widget;
        Render();
    }

    public Declarative(Context context, IAttributeSet? attrs, Widget widget) : base(context, attrs)
    {
        Widget = widget;
        Render();
    }

    public Declarative(Context context, IAttributeSet? attrs, int defStyleAttr, Widget widget) : base(context, attrs, defStyleAttr)
    {
        Widget = widget;
        Render();
    }

    public Declarative(Context context, IAttributeSet? attrs, int defStyleAttr, int defStyleRes, Widget widget) : base(context, attrs, defStyleAttr, defStyleRes)
    {
        Widget = widget;
        Render();
    }
}