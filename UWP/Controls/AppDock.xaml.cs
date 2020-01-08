using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using WeiPo.Common;
using WeiPo.ViewModels;
using System;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.DataTransfer;
using WeiPo.Services.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Controls
{
    public sealed partial class AppDock : INotifyPropertyChanged
    {
        private int _imageCount;

        public AppDock()
        {
            InitializeComponent();
            DataContext = DockViewModel.Instance;
            Singleton<BroadcastCenter>.Instance.Subscribe("status_create", delegate { StartComposing(); });
            Singleton<BroadcastCenter>.Instance.Subscribe("status_share", (sender, args) => StartComposing());
            Singleton<BroadcastCenter>.Instance.Subscribe("status_comment", (sender, args) => StartComposing());
            Singleton<BroadcastCenter>.Instance.Subscribe("post_weibo_complete",
                (sender, args) => StopComposing());
            Singleton<BroadcastCenter>.Instance.Subscribe("dock_image_count_changed", (sender, args) =>
            {
                if (args is int intArgs)
                {
                    _imageCount = intArgs;
                    ToggleImageTeachingTip();
                }
            });
            Singleton<BroadcastCenter>.Instance.Subscribe("share_add_image",
                delegate { StartComposing(); });
            Singleton<BroadcastCenter>.Instance.Subscribe("share_add_text",
                delegate { StartComposing(); });
        }
        
        public bool IsComposing { get; private set; }

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
            Visibility = Visibility.Visible;
            FullBackground.Visibility = Visibility.Visible;
            DockInput.Focus(FocusState.Programmatic);
        }

        public void StopComposing()
        {
            IsComposing = false;
            FullBackground.Visibility = Visibility.Collapsed;
            Visibility = Visibility.Collapsed;
            ViewModel.PostWeiboViewModel.ToCreateState();
            ToggleImageTeachingTip();
        }

        private void FullBackground_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StopComposing();
        }

        private void NotificationClick(object sender, RoutedEventArgs e)
        {
            Singleton<BroadcastCenter>.Instance.Send(this, "message_center_visible", true);
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
            ////trick to hide close button from image picker flyout
            if (sender is FrameworkElement element)
            {
                element.Loaded -= TeachGridLoaded;
                var button = element.FindAscendant<Button>();
                if (button != null)
                {
                    button.Visibility = Visibility.Collapsed;
                    button.RegisterPropertyChangedCallback(VisibilityProperty, (o, dp) =>
                    {
                        if (dp == VisibilityProperty && o is FrameworkElement frameworkElement && frameworkElement.Visibility == Visibility.Visible)
                        {
                            frameworkElement.Visibility = Visibility.Collapsed;
                        }
                    });
                }
                
            }
        }

        private void ToggleImageTeachingTip()
        {
            ImageTeachTip.IsOpen = _imageCount > 0 && Visibility == Visibility.Visible;
        }

        private async void TextBox_Paste(object sender, TextControlPasteEventArgs e)
        {
            var dataPackageView = Windows.ApplicationModel.DataTransfer.Clipboard.GetContent();
            if (dataPackageView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.Bitmap))
            {
                e.Handled = true;
                var bitmap = await dataPackageView.GetBitmapAsync();
                var file = await bitmap.SaveCacheFile();
                ViewModel.PostWeiboViewModel.AddImage(file);
            }
            else if (dataPackageView.Contains(Windows.ApplicationModel.DataTransfer.StandardDataFormats.StorageItems))
            {
                e.Handled = true;
                var files = (await dataPackageView.GetStorageItemsAsync())
                    .Where(item => item is StorageFile file && file.ContentType.Contains("image"))
                    .Select(it => it as StorageFile)
                    .ToArray();
                ViewModel.PostWeiboViewModel.AddImage(files);
            }
        }

        private bool _isCtrlDown;
        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Control)
            {
                _isCtrlDown = true;
            }
            if (_isCtrlDown && e.Key == Windows.System.VirtualKey.Enter)
            {
                ViewModel.PostWeiboViewModel.Commit();
            }
            (sender as TextBox).AcceptsReturn = !_isCtrlDown;
        }

        private void TextBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {

            if (e.Key == Windows.System.VirtualKey.Control)
            {
                _isCtrlDown = false;
                (sender as TextBox).AcceptsReturn = !_isCtrlDown;
            }
        }

        private void EmojiButton_Click(object sender, RoutedEventArgs e)
        {
            EmojiTeachTip.IsOpen = !EmojiTeachTip.IsOpen;
        }

        private void EmojiGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e.ClickedItem as EmojiModel;
            var index = DockInput.SelectionStart;
            ViewModel.PostWeiboViewModel.Content = DockInput.Text.Insert(index, item.Value);
            DockInput.SelectionStart = index + item.Value.Length;
        }
    }
}