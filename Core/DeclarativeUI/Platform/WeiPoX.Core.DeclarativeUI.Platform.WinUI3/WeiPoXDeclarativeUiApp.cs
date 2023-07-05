using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;
using WeiPoX.Core.Lifecycle;

namespace WeiPoX.Core.DeclarativeUI.Platform.WinUI3;

public abstract class WeiPoXDeclarativeUiApp : Application
{
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var renders = CreateRenderers();
        foreach (var (type, renderer) in renders)
        {
            RendererPool.RegisterRenderer(type, renderer);
        }
        new Window
        {
            Content = new DeclarativeView
            {
                Widget = new AppWidget
                {
                    App = CreateWidget(),
                }
            },
        }.Activate();
    }
    
    protected virtual Dictionary<Type, IRenderer<UIElement>> CreateRenderers()
    {
        return new Dictionary<Type, IRenderer<UIElement>>();
    }
    
    protected abstract Widget CreateWidget();
    
}