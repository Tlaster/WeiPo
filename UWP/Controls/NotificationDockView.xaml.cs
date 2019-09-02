using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WeiPo.Common;
using WeiPo.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Controls
{
    public sealed partial class NotificationDockView
    {
        public NotificationDockView()
        {
            this.InitializeComponent();
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
