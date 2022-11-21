namespace WeiPoX.Core.DeclarativeUI.Avalonia.Renderer;

internal class ButtonRenderer : RendererObject<Button, global::Avalonia.Controls.Button>
{
    protected override void Update(global::Avalonia.Controls.Button control, Button widget)
    {
        control.Command = widget.OnClick;
    }
}