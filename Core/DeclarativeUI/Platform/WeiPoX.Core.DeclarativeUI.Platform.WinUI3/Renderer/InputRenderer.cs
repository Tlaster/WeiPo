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
        control.UpdateState(widget.State);
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
    
    public void UpdateState(InputState state)
    {
        if (Text != state.Text)
        {
            Text = state.Text;
        }

        if (SelectionStart != state.SelectionStart || SelectionLength != state.SelectionEnd - state.SelectionStart)
        {
            SelectionStart = Math.Clamp(state.SelectionStart, 0, state.Text.Length);
            SelectionLength = Math.Clamp(state.SelectionEnd - state.SelectionStart, 0, state.Text.Length);
        }
    }

    private void OnTextChanging(TextBox sender, TextBoxTextChangingEventArgs args)
    {
        if (args.IsContentChanging)
        {
            TextChangedCallback?.Invoke(new InputState(Text, SelectionStart, SelectionStart + SelectionLength));
        }
    }
}