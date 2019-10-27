using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using WeiPo.Common;
using WeiPo.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Controls
{
    public sealed partial class AccountMessagingCenterDock : UserControl
    {
        public AccountMessagingCenterDock()
        {
            InitializeComponent();
            Singleton<BroadcastCenter>.Instance.Subscribe("message_center_visible", (sender, args) =>
            {
                if (args is bool boolArgs)
                {
                    Visibility = boolArgs ? Visibility.Visible : Visibility.Collapsed;
                    Singleton<BroadcastCenter>.Instance.Send(this, "request_dock_visible", !boolArgs);
                }
            });
            Singleton<BroadcastCenter>.Instance.Subscribe("message_center_to", (sender, args) =>
            {
                if (args is string strArgs)
                {
                    MessageNavigationView.SelectedItem =
                        ViewModel.Source.FirstOrDefault(it => it.Id == strArgs);
                }
            });
            DataContext = ViewModel;
        }

        public AccountMessagingCenterDockViewModel ViewModel { get; } = new AccountMessagingCenterDockViewModel();

        public bool OnBackPress()
        {
            if (Visibility == Visibility.Visible)
            {
                Singleton<BroadcastCenter>.Instance.Send(this, "message_center_visible", false);
                return true;
            }

            return false;
        }

        private void DockBackground_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;

            Singleton<BroadcastCenter>.Instance.Send(this, "message_center_visible", false);
        }
    }
}