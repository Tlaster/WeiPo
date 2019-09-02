using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using WeiPo.Common.Collection;
using RefreshContainer = Windows.UI.Xaml.Controls.RefreshContainer;
using RefreshRequestedEventArgs = Windows.UI.Xaml.Controls.RefreshRequestedEventArgs;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace WeiPo.Controls
{
    public sealed class ListViewEx : Control
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(object), typeof(ListViewEx),
            new PropertyMetadata(default, PropertyChangedCallback));

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register(
            nameof(ItemTemplate), typeof(object), typeof(ListViewEx), new PropertyMetadata(default));

        public static readonly DependencyProperty LayoutProperty = DependencyProperty.Register(
            nameof(Layout), typeof(Layout), typeof(ListViewEx), new PropertyMetadata(default(Layout)));

        private bool _isLoading;
        private RefreshContainer _refresher;

        private ScrollViewer _scrollViewer;

        public ListViewEx()
        {
            DefaultStyleKey = typeof(ListViewEx);
        }

        public object ItemsSource
        {
            get => GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object ItemTemplate
        {
            get => GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public Layout Layout
        {
            get => (Layout) GetValue(LayoutProperty);
            set => SetValue(LayoutProperty, value);
        }

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ItemsSourceProperty) (d as ListViewEx).OnItemsSourceChanged();
        }

        private void OnItemsSourceChanged()
        {
            if (ItemsSource is ISupportRefresh refresh)
                _refresher?.RequestRefresh();
            //refresh.Refresh();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
            if (_scrollViewer != null) _scrollViewer.ViewChanged += ScrollViewerOnViewChanged;
            _refresher = GetTemplateChild("RefreshContainer") as RefreshContainer;
            if (_refresher != null) _refresher.RefreshRequested += RefresherOnRefreshRequested;
            Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                _refresher.RequestRefresh());
        }

        private async void RefresherOnRefreshRequested(RefreshContainer sender, RefreshRequestedEventArgs args)
        {
            if (_isLoading) return;

            if (ItemsSource is ISupportRefresh refresh)
            {
                _isLoading = true;
                var def = args.GetDeferral();
                await refresh.Refresh();
                def.Complete();
                _isLoading = false;
                Task.Delay(100).ContinueWith(it => Dispatcher.RunAsync(CoreDispatcherPriority.Low, TryLoadIfNotFill));
            }
        }
        private async void TryLoadIfNotFill()
        {
            if (_isLoading)
            {
                return;
            }

            if (_scrollViewer.ScrollableHeight > ActualHeight)
            {
                return;
            }

            if (!(ItemsSource is ISupportIncrementalLoading loading) || !loading.HasMoreItems)
            {
                return;
            }
            _isLoading = true;
            await loading.LoadMoreItemsAsync(20);
            _isLoading = false;

        }

        private async void ScrollViewerOnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate && !_isLoading)
            {
                var scroller = (ScrollViewer) sender;
                var distanceToEnd = scroller.ExtentHeight - (scroller.VerticalOffset + scroller.ViewportHeight);
                // trigger if within 2 viewports of the end
                if (distanceToEnd <= 2.0 * scroller.ViewportHeight
                    && ItemsSource is ISupportIncrementalLoading loading && loading.HasMoreItems)
                {
                    _isLoading = true;
                    await loading.LoadMoreItemsAsync(20);
                    _isLoading = false;
                }
            }
        }
    }
}