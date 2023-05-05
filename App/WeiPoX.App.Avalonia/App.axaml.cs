using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;

namespace WeiPoX.App.Avalonia;

public class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            // ExpressionObserver.DataValidators.RemoveAll(x => x is DataAnnotationsValidationPlugin);
            // desktop.MainWindow = new MainWindow();
            desktop.MainWindow = new Window
            {
                Content = new ScrollViewer
                {
                    Content = new ItemsRepeater
                    {
                        ItemsSource = Enumerable.Range(0, 100).ToList(),
                        ItemTemplate = new FuncDataTemplate<int>((i, _) => new TextBlock
                        {       
                            [!TextBlock.TextProperty] = new Binding(),
                        }, true)
                    }
                }
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}