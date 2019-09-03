using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            Singleton<MessagingCenter>.Instance.Subscribe("message_center_visible", (sender, args) =>
            {
                if (args is bool boolArgs)
                {
                    Visibility = boolArgs ? Visibility.Visible : Visibility.Collapsed;
                }
            });
            DataContext = ViewModel;
        }

        public AccountMessagingCenterDockViewModel ViewModel { get; } = new AccountMessagingCenterDockViewModel();

        private void DockBackground_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            e.Handled = true;
            
            Singleton<MessagingCenter>.Instance.Send(this, "message_center_visible", false);
            Singleton<MessagingCenter>.Instance.Send(this, "dock_visible", true);
        }

        public bool OnBackPress()
        {
            if (Visibility == Visibility.Visible)
            {
                Visibility = Visibility.Collapsed;
                return true;
            }
            return false;
        }
    }
}