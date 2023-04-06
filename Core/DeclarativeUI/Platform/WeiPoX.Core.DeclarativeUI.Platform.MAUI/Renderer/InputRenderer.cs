using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

internal class InputRenderer : RendererObject<Input, WeiPoXTextBox>
{
    protected override void Update(WeiPoXTextBox control, Input widget)
    {
        control.Updating = true;
        control.UpdateState(widget.State);
        control.TextChangedCallback = widget.OnStateChanged;
        control.Updating = false;
    }
}

internal class WeiPoXTextBox : Entry
{
    public bool Updating { get; set; }
    public Action<InputState>? TextChangedCallback { get; set; }
    public WeiPoXTextBox()
    {
        TextChanged += OnTextChanged;
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (Updating)
        {
            return;
        }
        
        TextChangedCallback?.Invoke(new InputState(e.NewTextValue, SelectionStart: CursorPosition, SelectionEnd: CursorPosition + SelectionLength));
    }

    public void UpdateState(InputState state)
    {
        if (Text != state.Text)
        {
            Text = state.Text;
        }

        if (CursorPosition != state.SelectionStart || SelectionLength != state.SelectionEnd - state.SelectionStart)
        {
            CursorPosition = state.SelectionStart;
            SelectionLength = state.SelectionEnd - state.SelectionStart;
            // SelectionStart = Math.Clamp(state.SelectionStart, 0, state.Text.Length);
            // SelectionEnd = Math.Clamp(state.SelectionEnd, 0, state.Text.Length);
        }
    }
}