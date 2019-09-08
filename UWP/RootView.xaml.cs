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
            MessageCenterDock.RegisterPropertyChangedCallback(VisibilityProperty,
                (sender, e) => { UpdateNavigationBackButton(); });
            Singleton<MessagingCenter>.Instance.Subscribe("login_completed",
                (sender, args) => RootContainer.Navigate<TimelineActivity>());
            Singleton<MessagingCenter>.Instance.Subscribe("status_clicked", (sender, args) => { });
            Singleton<MessagingCenter>.Instance.Subscribe("user_clicked",
                (sender, args) => { RootContainer.Navigate(typeof(UserActivity), args); });
            Singleton<MessagingCenter>.Instance.Subscribe("status_like", (sender, args) => { });
            Singleton<MessagingCenter>.Instance.Subscribe("image_clicked",
                (sender, args) => RootContainer.Navigate<ImageActivity>(args));
            Singleton<MessagingCenter>.Instance.Subscribe("video_clicked",
                (sender, args) => RootContainer.Navigate<VideoActivity>(args));
            Singleton<MessagingCenter>.Instance.Subscribe("request_dock_visible", (sender, args) =>
            {
                if (args is bool boolArgs)
                {
                    Singleton<MessagingCenter>.Instance.Send(this, "dock_visible", boolArgs && RootContainer.CurrentActivity is TimelineActivity);
                }
            });
            RootContainer.BackStackChanged += RootContainerOnBackStackChanged;
            RootContainer.Navigate(typeof(LoginActivity));
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!Dock.OnBackPress() && !MessageCenterDock.OnBackPress() && RootContainer.CanGoBack)
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
            Singleton<MessagingCenter>.Instance.Send(this, "message_center_visible", false);
            Singleton<MessagingCenter>.Instance.Send(this, "dock_visible",
                RootContainer.CurrentActivity is TimelineActivity);
        }

        private void UpdateNavigationBackButton()
        {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                RootContainer.CanGoBack || MessageCenterDock.Visibility == Visibility.Visible
                    ? AppViewBackButtonVisibility.Visible
                    : AppViewBackButtonVisibility.Collapsed;
        }
    }
}