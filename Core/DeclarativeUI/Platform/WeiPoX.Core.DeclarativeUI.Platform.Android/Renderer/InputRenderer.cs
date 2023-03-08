using Android.Content;
using Google.Android.Material.TextField;
using Java.Lang;
using WeiPoX.Core.DeclarativeUI.Widgets;
using Math = System.Math;

namespace WeiPoX.Core.DeclarativeUI.Platform.Android.Renderer;

internal class InputRenderer : RendererObject<Input, WeiPoXTextInputEditText>
{
    public InputRenderer(Context context) : base(context)
    {
    }

    protected override WeiPoXTextInputEditText Create(Context context)
    {
        return new WeiPoXTextInputEditText(context);
    }

    protected override void Update(WeiPoXTextInputEditText control, Input widget)
    {
        control.Updating = true;
        // TODO: Japanese input method is not working
        // take the react native code as reference
        // https://github.com/facebook/react-native/blob/b73dd6726d88ca4d8ffe71f69cd7a7f668582f21/ReactAndroid/src/main/java/com/facebook/react/views/textinput/ReactEditText.java#L679
        control.Text = widget.State.Text;
        control.SetSelection(
            Math.Clamp(widget.State.SelectionStart, 0, widget.State.Text.Length),
            Math.Clamp(widget.State.SelectionEnd, 0, widget.State.Text.Length)
        );
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
