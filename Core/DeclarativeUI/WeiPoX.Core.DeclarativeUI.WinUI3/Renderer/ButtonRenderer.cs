using System;
using ABI.System.Windows.Input;
using WeiPoX.Core.DeclarativeUI.Widget;

namespace WeiPoX.Core.DeclarativeUI.WinUI3.Renderer;

internal class ButtonRenderer : RendererObject<Button, Microsoft.UI.Xaml.Controls.Button>
{
    protected override void Update(Microsoft.UI.Xaml.Controls.Button control, Button widget)
    {
        control.Command = new RelayCommand(widget.OnClick);
    }
}

internal class RelayCommand : ICommand
{
    private readonly Action _action;

    public RelayCommand(Action action)
    {
        _action = action;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _action();
    }

    public event EventHandler? CanExecuteChanged;
}