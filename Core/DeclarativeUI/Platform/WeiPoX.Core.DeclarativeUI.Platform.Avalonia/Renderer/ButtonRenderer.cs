using System.Windows.Input;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Renderer;

internal class ButtonRenderer : RendererObject<Button, global::Avalonia.Controls.Button>
{
    protected override void Update(global::Avalonia.Controls.Button control, Button widget)
    {
        control.Command = new RelayCommand(widget.OnClick);
        control.Content = widget.Text;
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
#pragma warning disable 67
    public event EventHandler? CanExecuteChanged;
#pragma warning restore 67
}