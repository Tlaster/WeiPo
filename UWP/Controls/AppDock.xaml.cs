using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using WeiPo.Common;
using WeiPo.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Controls
{
    public sealed partial class AppDock : INotifyPropertyChanged
    {
        private bool _isPointerOverHeader;
        private bool _isTextBoxFocused;
        private bool _keepDockExpanded = true;
        private bool _isComposing = false;
        private int _imageCount = 0;

        public AppDock()
        {
            InitializeComponent();
            Singleton<MessagingCenter>.Instance.Subscribe("dock_expand", (sender, args) =>
            {
                if (args is bool boolArgs)
                {
                    _keepDockExpanded = boolArgs;
                    ToggleHeader();
                }
            });
            Singleton<MessagingCenter>.Instance.Subscribe("status_share", (sender, args) =>
            {
                ComposingPost();
            });
            Singleton<MessagingCenter>.Instance.Subscribe("status_comment", (sender, args) =>
            {
                ComposingPost();
            });
            Singleton<MessagingCenter>.Instance.Subscribe("dock_shadow", (sender, args) =>
            {
                if (args is bool boolArgs)
                {
                    HeaderShadows.Opacity = boolArgs ? 1f: 0f;
                }
            });
            Singleton<MessagingCenter>.Instance.Subscribe("dock_visible", (sender, args) =>
            {
                if (args is bool booArgs)
                {
                    StopComposing();
                    Visibility = booArgs ? Visibility.Visible : Visibility.Collapsed;
                    ToggleImageTeachingTip();
                }
            });
            Singleton<MessagingCenter>.Instance.Subscribe("post_weibo_complete", (sender, args) =>
            {
                StopComposing();
            });
            Singleton<MessagingCenter>.Instance.Subscribe("dock_image_count_changed", (sender, args) =>
            {
                if (args is int intArgs)
                {
                    _imageCount = intArgs;
                    ToggleImageTeachingTip();
                    ToggleHeader();
                }
            });
        }

        private void ToggleImageTeachingTip()
        {
            ImageTeachTip.IsOpen = _imageCount > 0 && Visibility == Visibility.Visible;
        }

        public bool IsHeaderOpened { get; private set; } = true;

        private Visibility _prevVisibility;
        private void ComposingPost()
        {
            _isComposing = true;
            _prevVisibility = Visibility;
            if (_prevVisibility == Visibility.Collapsed)
            {
                Visibility = Visibility.Visible;
                HeaderShadows.Opacity = Visibility == Visibility.Visible ? 1f: 0f;
            }
            FullBackground.Visibility = Visibility.Visible;
            ToggleHeader();
        }

        private void StopComposing()
        {
            _isComposing = false;
            ToggleHeader();
            Visibility = _prevVisibility;
            HeaderShadows.Opacity = Visibility == Visibility.Visible ? 1f: 0f;
            FullBackground.Visibility = Visibility.Collapsed;
            Singleton<MessagingCenter>.Instance.Send(this, "clear_dock_compose");
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

        private void ToggleHeader()
        {
            if (_keepDockExpanded || _isPointerOverHeader || _isTextBoxFocused || _isComposing || _imageCount > 0)
                ExpandHeader();
            else
                CloseHeader();
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

        private void FullBackground_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StopComposing();
        }

        private void TeachGridLoaded(object sender, RoutedEventArgs e)
        {
            //trick to hide close button from image picker flyout
            if (sender is FrameworkElement element)
            {
                var button = element.FindAscendant<Button>();
                if (button != null)
                {
                    button.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void RemoveImageClicked(object sender, RoutedEventArgs e)
        {
            var file = (sender as FrameworkElement)?.DataContext as StorageFile;
            (DataContext as DockViewModel)?.PostWeiboViewModel.Files.Remove(file);
        }
    }
}