using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using WeiPo.Activities;
using WeiPo.Activities.User;
using WeiPo.Common;
using WeiPo.Controls;

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
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            
            Init();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (RootContainer.CanGoBack)
            {
                RootContainer.GoBack();
            }
        }

        private void Init()
        {
            Singleton<MessagingCenter>.Instance.Subscribe("status_clicked", (sender, args) =>
            {
            });
            Singleton<MessagingCenter>.Instance.Subscribe("user_clicked", (sender, args) =>
            {
                RootContainer.Navigate(typeof(UserActivity), args);
            });
            Singleton<MessagingCenter>.Instance.Subscribe("status_share", (sender, args) =>
            {
                
            });
            Singleton<MessagingCenter>.Instance.Subscribe("status_comment", (sender, args) =>
            {
                
            });
            Singleton<MessagingCenter>.Instance.Subscribe("status_like", (sender, args) =>
            {
                
            });
            RootContainer.BackStackChanged += RootContainerOnBackStackChanged;
            RootContainer.Navigate(typeof(LoginActivity));
        }


        private void RootContainerOnBackStackChanged(object sender, EventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = RootContainer.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }


        private void OnCoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Height = sender.Height;
        }
    }
}
