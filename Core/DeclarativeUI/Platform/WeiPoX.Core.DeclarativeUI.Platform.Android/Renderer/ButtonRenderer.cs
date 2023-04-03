using System.Windows.Input;
using Android.Content;
using Android.Views;
using Google.Android.Material.Button;
using Java.Interop;
using ReactiveUI;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class ButtonRenderer : RendererObject<WeiPoX.Core.DeclarativeUI.Widgets.Button, Button>
{
    public ButtonRenderer(Context context) : base(context)
    {
    }

    protected override Button Create(Context context, RendererContext<View> rendererContext)
    {
        var button = new MaterialButton(context);
        button.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
        return button;
    }

    protected override void Update(Button control, Widgets.Button widget)
    {
        control.SetOnClickListener(new OnClickListener(widget.OnClick));
        control.Text = widget.Text;
    }
}

internal class OnClickListener : Java.Lang.Object, View.IOnClickListener
{
    private readonly Action _command;

    public OnClickListener(Action command)
    {
        _command = command;
    }

    public void OnClick(View? v)
    {
        _command.Invoke();
    }
}

internal class OnLongClickListener : Java.Lang.Object, View.IOnLongClickListener
{
    private readonly Action _command;

    public OnLongClickListener(Action command)
    {
        _command = command;
    }

    public bool OnLongClick(View? v)
    {
        _command.Invoke();
        return true;
    }
}

internal class OnDoubleTapListener : Java.Lang.Object, View.IOnTouchListener
{
    private readonly Action _command;

    public OnDoubleTapListener(Action command)
    {
        _command = command;
    }

    public bool OnTouch(View? v, MotionEvent? e)
    {
        if (e?.Action == MotionEventActions.Up)
        {
            _command.Invoke();
        }

        return true;
    }
}

