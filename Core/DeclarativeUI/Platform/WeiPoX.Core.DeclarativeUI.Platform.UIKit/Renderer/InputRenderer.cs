using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class InputRenderer : RendererObject<Input, WeiPoXTextField>
{
    protected override void Update(WeiPoXTextField control, Input widget)
    {
        control.Updating = true;
        control.UpdateState(widget.State);
        control.TextChangedCallback = widget.OnStateChanged;
        control.Updating = false;
    }
}

internal class WeiPoXTextField : UITextField
{
    public bool Updating { get; set; }
    public Action<InputState>? TextChangedCallback { get; set; }

    public WeiPoXTextField()
    {
        EditingChanged += OnEditingChanged;
    }
    
    public void UpdateState(InputState state)
    {
        if (Text != state.Text)
        {
            Text = state.Text;
        }

        if (SelectedTextRange != null)
        {
            var start = GetOffsetFromPosition(BeginningOfDocument, SelectedTextRange.Start);
            var end = GetOffsetFromPosition(BeginningOfDocument, SelectedTextRange.End);
            if (start != state.SelectionStart || end != state.SelectionEnd)
            {
                SelectedTextRange = GetTextRange(
                    GetPosition(BeginningOfDocument, state.SelectionStart),
                    GetPosition(BeginningOfDocument, state.SelectionEnd));
            }
        }
    }

    private void OnEditingChanged(object? sender, EventArgs e)
    {
        if (Updating)
        {
            return;
        }

        var range = SelectedTextRange;
        if (range != null)
        {
            var start = GetOffsetFromPosition(BeginningOfDocument, range.Start);
            var end = GetOffsetFromPosition(BeginningOfDocument, range.End);
            TextChangedCallback?.Invoke(new InputState(Text ?? "", Convert.ToInt32(start), Convert.ToInt32(end)));
        }
        else
        {
            TextChangedCallback?.Invoke(new InputState(Text ?? "", 0, 0));
        }
    }
}