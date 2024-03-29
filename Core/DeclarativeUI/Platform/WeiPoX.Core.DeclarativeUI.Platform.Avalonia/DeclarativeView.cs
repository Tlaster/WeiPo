﻿using Avalonia.Controls;
using Avalonia.Threading;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.Avalonia.Internal;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Platform.Avalonia;

public class DeclarativeView : UserControl
{
    private readonly DeclarativeCore<Control> _core;

    public DeclarativeView(IBuildOwner? buildOwner = null)
    {
        _core = new DeclarativeCore<Control>(new WidgetBuilder(), UpdateChild, RunInUi, buildOwner);
    }

    private void RunInUi(Action obj)
    {
        obj.Invoke();
        // Dispatcher.UIThread.InvokeAsync(obj);
    }

    public Widget? Widget
    {
        get => _core.Widget;
        set => _core.Widget = value;
    }
    
    internal void UpdateChild(Control control)
    {
        if (!Equals(Content, control))
        {
            Content = control;
        }
    }
}