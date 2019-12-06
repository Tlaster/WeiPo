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
            //Singleton<BroadcastCenter>.Instance.Subscribe("dock_visible", (sender, args) =>
            //{
            //    if (args is bool booArgs)
            //    {
            //        Visibility = booArgs ? Visibility.Visible : Visibility.Collapsed;
            //    }
            //});
            DataContext = NotificationViewModel.Instance;
        }

        public NotificationViewModel ViewModel => NotificationViewModel.Instance;

        private void MentionTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            SendMessage("Mention");
        }

        private void MentionCommentTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            SendMessage("MentionComment");
        }

        private void CommentTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            SendMessage("Comment");
        }

        private void FansTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {

        }

        private void DmTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            SendMessage("DirectMessage");
        }

        private void SendMessage(string message)
        {
            Singleton<BroadcastCenter>.Instance.Send(this, "message_center_visible", true);
            Singleton<BroadcastCenter>.Instance.Send(this, "message_center_to", message);
        }
    }
}