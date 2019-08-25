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
    public sealed partial class TimelineActivity : INotifyPropertyChanged
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
                ToggleHeader();
                if (_scrollViewer.VerticalOffset < 5d)
                {
                    HeaderContainer.ShadowOpacity = 0f;
                }
                else
                {
                    HeaderContainer.ShadowOpacity = 0.6f;
                }
            }
        }

        private void OnCoreTitleBarOnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            //Header.Margin = new Thickness(0, sender.Height, 0, 0);
        }

        private void UIElement_OnPointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _isPointerOverHeader = true;
            ToggleHeader();
        }

        private void UIElement_OnPointerExited(object sender, PointerRoutedEventArgs e)
        {
            _isPointerOverHeader = false;
            ToggleHeader();
        }

        public bool IsHeaderOpened { get; private set; } = true;
        private bool _isPointerOverHeader = false;
        private bool _isTextBoxFocused = false;
        private void ToggleHeader()
        {
            if (_scrollViewer.VerticalOffset < 5d || _isPointerOverHeader || _isTextBoxFocused)
            {
                ExpandHeader();
            } 
            else
            {
                CloseHeader();
            }
        }

        private void CloseHeader()
        {
            IsHeaderOpened = false;
        }

        private void ExpandHeader()
        {
            IsHeaderOpened = true;
        }

        private void UIElement_OnGotFocus(object sender, RoutedEventArgs e)
        {
            _isTextBoxFocused = true;
            ToggleHeader();
        }

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            _isTextBoxFocused = false;
            ToggleHeader();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
