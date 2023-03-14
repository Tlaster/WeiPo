using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Styling;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

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

internal class WeiPoXTextBox : TextBox, IStyleable
{
    Type IStyleable.StyleKey => typeof(TextBox);
    public bool Updating { get; set; }
    public Action<InputState>? TextChangedCallback { get; set; }
    public WeiPoXTextBox()
    {
        TextChanging += OnTextChanging;
    }
    
    public void UpdateState(InputState state)
    {
        if (Text != state.Text)
        {
            Text = state.Text;
        }

        if (SelectionStart != state.SelectionStart || SelectionEnd != state.SelectionEnd)
        {
            SelectionStart = Math.Clamp(state.SelectionStart, 0, state.Text.Length);
            SelectionEnd = Math.Clamp(state.SelectionEnd, 0, state.Text.Length);
        }
    }

    private void OnTextChanging(object? sender, TextChangingEventArgs e)
    {
        if (Updating)
        {
            return;
        }

        if (Text != null)
        {
            TextChangedCallback?.Invoke(new InputState(Text, SelectionStart, SelectionEnd));
        }
    }
}