using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using WeiPoX.Core.DeclarativeUI.Animation;
using WeiPoX.Core.DeclarativeUI.Animation.Platform.WinUI3;
using WeiPoX.Core.DeclarativeUI.Internal;
using WeiPoX.Core.DeclarativeUI.Platform.WinUI3;
using WeiPoX.Core.DeclarativeUI.Sample.Core;
using WeiPoX.Core.DeclarativeUI.Widgets;

namespace WeiPoX.Core.DeclarativeUI.Sample.WinUI3
{
    public partial class App : WeiPoXDeclarativeUiApp
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override Widget CreateWidget()
        {
            return new SampleApp();
        }

        protected override Dictionary<Type, IRenderer<UIElement>> CreateRenderers()
        {
            return new Dictionary<Type, IRenderer<UIElement>>
            {
                { typeof(PlatformAnimated), new PlatformAnimatedRenderer() },
            };
        }
    }
}
