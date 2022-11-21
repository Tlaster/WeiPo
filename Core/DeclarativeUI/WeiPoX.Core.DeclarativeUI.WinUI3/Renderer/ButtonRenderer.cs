namespace WeiPoX.Core.DeclarativeUI.WinUI3.Renderer;

internal class ButtonRenderer : RendererObject<Button, Microsoft.UI.Xaml.Controls.Button>
{
    protected override void Update(Microsoft.UI.Xaml.Controls.Button control, Button widget)
    {
        control.Command = widget.OnClick;
    }
}