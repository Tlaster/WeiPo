using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WeiPo.Activities;
using WeiPo.Common;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo
{
    public sealed partial class RootView : Grid
    {
        public RootView()
        {
            this.InitializeComponent();
            CoreApplication.GetCurrentView().TitleBar.Also(it =>
            {
                it.LayoutMetricsChanged += OnCoreTitleBarOnLayoutMetricsChanged;
                it.ExtendViewIntoTitleBar = true;
            });
            Window.Current.SetTitleBar(TitleBar);
            ApplicationView.GetForCurrentView().TitleBar.Also(it =>
            {
                it.ButtonBackgroundColor = Colors.Transparent;
                it.ButtonInactiveBackgroundColor = Colors.Transparent;
            });
            RootContainer.Navigate(typeof(LoginActivity));
        }

        private void OnCoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Height = sender.Height;
        }
    }
}
