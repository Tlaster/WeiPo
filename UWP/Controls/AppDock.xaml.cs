using System.ComponentModel;
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
        private int _imageCount;
        private bool _isPointerOverHeader;
        private bool _isTextBoxFocused;
        private bool _keepDockExpanded = true;

        private Visibility _prevVisibility;

        public AppDock()
        {
            InitializeComponent();
            DataContext = DockViewModel.Instance;
            Singleton<MessagingCenter>.Instance.Subscribe("dock_expand", (sender, args) =>
            {
                if (args is bool boolArgs)
                {
                    _keepDockExpanded = boolArgs;
                    ToggleHeader();
                }
            });
            Singleton<MessagingCenter>.Instance.Subscribe("status_share", (sender, args) => StartComposing());
            Singleton<MessagingCenter>.Instance.Subscribe("status_comment", (sender, args) => StartComposing());
            Singleton<MessagingCenter>.Instance.Subscribe("dock_visible", (sender, args) =>
            {
                if (args is bool booArgs)
                {
                    StopComposing();
                    Visibility = booArgs ? Visibility.Visible : Visibility.Collapsed;
                    ToggleImageTeachingTip();
                }
            });
            Singleton<MessagingCenter>.Instance.Subscribe("post_weibo_complete",
                (sender, args) => StopComposing());
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

        public bool IsComposing { get; private set; }

        public bool IsHeaderOpened { get; private set; } = true;

        public DockViewModel ViewModel => DockViewModel.Instance;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool OnBackPress()
        {
            if (IsComposing)
            {
                StopComposing();
                return true;
            }

            return false;
        }

        public void StartComposing()
        {
            IsComposing = true;
            _prevVisibility = Visibility;
            if (_prevVisibility == Visibility.Collapsed)
            {
                Visibility = Visibility.Visible;
            }

            FullBackground.Visibility = Visibility.Visible;
            ToggleHeader();
        }

        public void StopComposing()
        {
            IsComposing = false;
            ToggleHeader();
            Visibility = _prevVisibility;
            FullBackground.Visibility = Visibility.Collapsed;
            ViewModel.PostWeiboViewModel.ToCreateState();
            //Singleton<MessagingCenter>.Instance.Send(this, "clear_dock_compose");
        }

        private void CloseHeader()
        {
            IsHeaderOpened = false;
        }

        private void ExpandHeader()
        {
            IsHeaderOpened = true;
        }

        private void FullBackground_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StopComposing();
        }

        private void NotificationClick(object sender, RoutedEventArgs e)
        {
            Singleton<MessagingCenter>.Instance.Send(this, "message_center_visible", true);
            Singleton<MessagingCenter>.Instance.Send(this, "dock_visible", false);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void RemoveImageClicked(object sender, RoutedEventArgs e)
        {
            var file = (sender as FrameworkElement)?.DataContext as StorageFile;
            ViewModel.PostWeiboViewModel.Files.Remove(file);
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

        private void ToggleHeader()
        {
            if (_keepDockExpanded || _isPointerOverHeader || _isTextBoxFocused || IsComposing || _imageCount > 0)
            {
                ExpandHeader();
            }
            else
            {
                CloseHeader();
            }
        }

        private void ToggleImageTeachingTip()
        {
            ImageTeachTip.IsOpen = _imageCount > 0 && Visibility == Visibility.Visible;
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
    }
}