using System;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WeiPo.Common;
using WeiPo.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeiPo.Activities
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TimelineActivity
    {
        public TimelineActivity()
        {
            this.InitializeComponent();
        }

        protected override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            this.Container.ClearBackStack();

            CoreApplication.GetCurrentView().TitleBar.Also(it =>
            {
                it.LayoutMetricsChanged += OnCoreTitleBarOnLayoutMetricsChanged;
            });
        }

        private void OnCoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            //Header.Margin = new Thickness(0, sender.Height, 0, 0);
        }

        private async void ScrollViewer_OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                var scroller = (ScrollViewer)sender;
                var distanceToEnd = scroller.ExtentHeight - (scroller.VerticalOffset + scroller.ViewportHeight);
                var viewModel = DataContext as TimelineViewModel;
                // trigger if within 2 viewports of the end
                if (distanceToEnd <= 2.0 * scroller.ViewportHeight
                    && viewModel.Timeline.HasMoreItems && !viewModel.Timeline.IsLoading)
                {
                    await viewModel.Timeline.LoadMoreItemsAsync(20);
                }
            }
        }
    }
}
