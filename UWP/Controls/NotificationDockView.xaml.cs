using Windows.UI.Xaml;
using WeiPo.Common;
using WeiPo.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Controls
{
    public sealed partial class NotificationDockView
    {
        public NotificationDockView()
        {
            InitializeComponent();
            Singleton<MessagingCenter>.Instance.Subscribe("dock_visible", (sender, args) =>
            {
                if (args is bool booArgs)
                {
                    Visibility = booArgs ? Visibility.Visible : Visibility.Collapsed;
                }
            });
            DataContext = NotificationViewModel.Instance;
        }

        public NotificationViewModel ViewModel => NotificationViewModel.Instance;
    }
}