using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.VisualStudio.TestPlatform.TestExecutor;

namespace WeiPoX.Core.DeclarativeUI.WinUI3.Test;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        UnitTestClient.CreateDefaultUI();

        var window = new Window
        {
            Content = new Frame()
        };

        // Ensure the current window is active
        window.Activate();

        // UITestMethodAttribute.DispatcherQueue = window.DispatcherQueue;

        // Replace back with e.Arguments when https://github.com/microsoft/microsoft-ui-xaml/issues/3368 is fixed
        UnitTestClient.Run(Environment.CommandLine);
    }
}