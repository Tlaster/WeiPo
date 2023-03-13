using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.UIKit.Renderer;

internal class InputRenderer : RendererObject<Input, WeiPoXTextField>
{
    protected override void Update(WeiPoXTextField control, Input widget)
    {
        control.Updating = true;
        control.Text = widget.State.Text;
        control.SelectedTextRange = control.GetTextRange(
            control.GetPosition(control.BeginningOfDocument, widget.State.SelectionStart),
            control.GetPosition(control.BeginningOfDocument, widget.State.SelectionEnd));
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