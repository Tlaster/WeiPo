using Android.Content;
using Android.Text;
using Android.Views;
using Google.Android.Material.TextField;
using Java.Lang;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Android.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using Math = System.Math;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class InputRenderer : RendererObject<Input, WeiPoXTextInputEditText>
{
    public InputRenderer(Context context) : base(context)
    {
    }

    protected override WeiPoXTextInputEditText Create(Context context, WidgetBuilder renderer)
    {
        return new WeiPoXTextInputEditText(context);
    }

    protected override void Update(WeiPoXTextInputEditText control, Input widget)
    {
        control.Updating = true;
        control.UpdateState(widget.State);
        control.TextChangedCallback = widget.OnStateChanged;
        control.Updating = false;
    }
}

internal class WeiPoXTextInputEditText : TextInputEditText
{
    public WeiPoXTextInputEditText(Context context) : base(context)
    {
    }

    public bool Updating { get; set; }
    public Action<InputState>? TextChangedCallback { get; set; }

    public void UpdateState(InputState state)
    {
        if (Text != state.Text)
        {
            EditableText?.Replace(0, EditableText.Length(), state.Text);
        }

        if (SelectionStart != state.SelectionStart || SelectionEnd != state.SelectionEnd)
        {
            SetSelection(
                Math.Clamp(state.SelectionStart, 0, state.Text.Length),
                Math.Clamp(state.SelectionEnd, 0, state.Text.Length)
            );
        }
    }
    

    protected override void OnTextChanged(ICharSequence? text, int start, int lengthBefore, int lengthAfter)
    {
        base.OnTextChanged(text, start, lengthBefore, lengthAfter);
        if (Updating)
        {
            return;
        }

        if (text != null)
        {
            TextChangedCallback?.Invoke(new InputState(text.ToString(), SelectionStart, SelectionEnd));
        }
    }
}
