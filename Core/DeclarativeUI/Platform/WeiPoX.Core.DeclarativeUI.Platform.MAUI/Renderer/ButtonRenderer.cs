using System.Windows.Input;
using MAUIButton = Microsoft.Maui.Controls.Button;
using WeiPoXButton = WeiPoX.Core.DeclarativeUI.Widgets.Button;

namespace WeiPoX.Core.DeclarativeUI.Platform.MAUI.Renderer;

internal class ButtonRenderer : RendererObject<WeiPoXButton, MAUIButton>
{
    protected override void Update(MAUIButton control, WeiPoXButton widget)
    {
        control.Command = new RelayCommand(widget.OnClick);
        control.Text = widget.Text;
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