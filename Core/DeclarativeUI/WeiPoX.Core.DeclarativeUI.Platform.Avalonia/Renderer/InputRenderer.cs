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
        control.Text = widget.State.Text;
        control.SelectionStart = Math.Clamp(widget.State.SelectionStart, 0, widget.State.Text.Length);
        control.SelectionEnd = Math.Clamp(widget.State.SelectionEnd, 0, widget.State.Text.Length);
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