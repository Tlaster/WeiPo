using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Behaviors;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Microsoft.Xaml.Interactivity;
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
        private ScrollViewer _scrollViewer;

        public TimelineActivity()
        {
            this.InitializeComponent();
        }

        protected override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            CoreApplication.GetCurrentView().TitleBar.Also(it =>
            {
                it.LayoutMetricsChanged += OnCoreTitleBarOnLayoutMetricsChanged;
            });
        }

        protected override void OnResume()
        {
            base.OnResume();
            //Singleton<MessagingCenter>.Instance.Send(this, "dock_visible", true);
        }

        protected override void OnPause()
        {
            base.OnPause();
            //Singleton<MessagingCenter>.Instance.Send(this, "dock_visible", false);
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

        private void ScrollViewerOnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                Singleton<MessagingCenter>.Instance.Send(this, "dock_expand", _scrollViewer.VerticalOffset < 5d);
                Singleton<MessagingCenter>.Instance.Send(this, "dock_shadow", _scrollViewer.VerticalOffset > 5d);
            }
        }

        private void OnCoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            //Header.Margin = new Thickness(0, sender.Height, 0, 0);
        }
    }
}
