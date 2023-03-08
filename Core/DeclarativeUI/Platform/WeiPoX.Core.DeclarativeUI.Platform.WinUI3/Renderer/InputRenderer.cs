using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Renderer;

internal class InputRenderer : RendererObject<Input, WeiPoXTextBox>
{
    protected override void Update(WeiPoXTextBox control, Input widget)
    {
        control.Updating = true;
        control.Text = widget.State.Text;
        control.SelectionStart = Math.Clamp(widget.State.SelectionStart, 0, widget.State.Text.Length);
        control.SelectionLength = Math.Clamp(widget.State.SelectionEnd - widget.State.SelectionStart, 0, widget.State.Text.Length);
        control.TextChangedCallback = widget.OnStateChanged;
        control.Updating = false;
    }
}

internal class WeiPoXTextBox : TextBox
{
    public bool Updating { get; set; }
    public Action<InputState>? TextChangedCallback { get; set; }
    public WeiPoXTextBox()
    {
        TextChanging += OnTextChanging;
    }

    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        if (args.IsContentChanging)
        {
            TextChangedCallback?.Invoke(new InputState(Text, SelectionStart, SelectionStart + SelectionLength));
        }
    }
}