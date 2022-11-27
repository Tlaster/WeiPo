using System.Windows.Input;
using WeiPoX.Core.DeclarativeUI.Widget;

namespace WeiPoX.Core.DeclarativeUI.Avalonia.Renderer;

internal class ButtonRenderer : RendererObject<Button, global::Avalonia.Controls.Button>
{
    protected override void Update(global::Avalonia.Controls.Button control, Button widget)
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