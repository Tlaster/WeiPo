using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using WeiPo.Common;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeiPo.Activities
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimelineActivity
    {

        public TimelineActivity()
        {
            InitializeComponent();
        }

        protected override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            Singleton<BroadcastCenter>.Instance.SubscribeWithPendingMessage("share_target_receive", async (sender, args) =>
            {
                if (args is ProtocolActivatedEventArgs protocolActivatedEventArgs)
                {
                    if (protocolActivatedEventArgs.Data.ContainsKey("image"))
                    {
                        var file = await SharedStorageAccessManager.RedeemTokenForFileAsync(
                            protocolActivatedEventArgs.Data["image"].ToString());
                        Singleton<BroadcastCenter>.Instance.Send(this, "share_add_image", file);
                    }

                    if (protocolActivatedEventArgs.Data.ContainsKey("text"))
                    {
                        var text = protocolActivatedEventArgs.Data["text"].ToString();
                        Singleton<BroadcastCenter>.Instance.Send(this, "share_add_text", text);
                    }
                }
            });
        }

        private void TimelinePivot_Loaded(object sender, RoutedEventArgs e)
        {
            TimelinePivot.FindDescendantByName("HeaderClipper").Visibility = Visibility.Collapsed;
            TimelinePivot.FindDescendantByName("LeftHeaderPresenter").Visibility = Visibility.Collapsed;
            TimelinePivot.FindDescendantByName("PreviousButton").Visibility = Visibility.Collapsed;
            TimelinePivot.FindDescendantByName("NextButton").Visibility = Visibility.Collapsed;
            TimelinePivot.FindDescendantByName("RightHeaderPresenter").Visibility = Visibility.Collapsed;
        }

        private void ShareNavigationViewItemTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            Singleton<BroadcastCenter>.Instance.Send(this, "status_create");
        }
    }
}