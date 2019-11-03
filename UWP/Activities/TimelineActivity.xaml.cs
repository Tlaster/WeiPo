using System;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private ScrollViewer _scrollViewer;

        public TimelineActivity()
        {
            InitializeComponent();
        }

        protected override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            CoreApplication.GetCurrentView().TitleBar.Also(it =>
            {
                it.LayoutMetricsChanged += OnCoreTitleBarOnLayoutMetricsChanged;
            });
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

        protected override void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            base.OnLoaded(sender, routedEventArgs);
            if (_scrollViewer == null)
            {
                _scrollViewer = TimelineListView.FindDescendant<ScrollViewer>();
                _scrollViewer.ViewChanged += ScrollViewerOnViewChanged;
            }
        }

        private void OnCoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            //Header.Margin = new Thickness(0, sender.Height, 0, 0);
        }

        private void ScrollViewerOnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                Singleton<BroadcastCenter>.Instance.Send(this, "dock_expand", _scrollViewer.VerticalOffset < 5d);
            }
        }
    }
}