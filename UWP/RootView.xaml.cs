using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WeiPo.Activities;
using WeiPo.Activities.User;
using WeiPo.Common;
using WeiPo.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo
{
    public sealed partial class RootView : Grid
    {
        public RootView()
        {
            InitializeComponent();
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
            RootContainer.Navigated += RootContainerOnNavigated;
            Init();
        }

        private void Init()
        {
            Singleton<BroadcastCenter>.Instance.Subscribe("login_completed",
                (sender, args) => RootContainer.Navigate<TimelineActivity>());
            Singleton<BroadcastCenter>.Instance.Subscribe("status_clicked", 
                (sender, args) => RootContainer.Navigate<StatusActivity>(args));
            Singleton<BroadcastCenter>.Instance.Subscribe("user_clicked",
                (sender, args) => { RootContainer.Navigate(typeof(UserActivity), args); });
            Singleton<BroadcastCenter>.Instance.Subscribe("status_like", (sender, args) => { });
            Singleton<BroadcastCenter>.Instance.Subscribe("image_clicked",
                (sender, args) => RootContainer.Navigate<ImageActivity>(args));
            Singleton<BroadcastCenter>.Instance.Subscribe("video_clicked",
                (sender, args) => RootContainer.Navigate<VideoActivity>(args));

            RegisterNotification("notification_new_fans", "FollowerCount");
            RegisterNotification("notification_new_mention_at", "MentionStatusCount");
            RegisterNotification("notification_new_mention_comment", "MentionCmtCount");
            RegisterNotification("notification_new_comment", "CmtCount");
            RegisterNotification("notification_new_dm", "DmCount");

            NotificationViewModel.Instance.Init();

            RootContainer.BackStackChanged += RootContainerOnBackStackChanged;
            RootContainer.Navigate(typeof(LoginActivity));
        }

        private void RegisterNotification(string name, string localizationName)
        {
            Singleton<BroadcastCenter>.Instance.Subscribe(name, (sender, args) =>
            {
                if (args is long longArgs)
                {
                    ToastNotificationSender.SendText(Localization.Format(localizationName, longArgs));
                }
            });
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!Dock.OnBackPress() && /*!MessageCenterDock.OnBackPress() &&*/ RootContainer.CanGoBack)
            {
                RootContainer.GoBack();
            }
        }


        private void OnCoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            TitleBar.Height = sender.Height;
        }

        private void RootContainerOnBackStackChanged(object sender, EventArgs e)
        {
            UpdateNavigationBackButton();
        }

        private void RootContainerOnNavigated(object sender, EventArgs e)
        {
            //Singleton<BroadcastCenter>.Instance.Send(this, "message_center_visible", false);
            //Singleton<BroadcastCenter>.Instance.Send(this, "dock_visible",
            //    RootContainer.CurrentActivity is TimelineActivity);
        }

        private void UpdateNavigationBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                RootContainer.CanGoBack /*|| MessageCenterDock.Visibility == Visibility.Visible*/
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
        }
    }
}