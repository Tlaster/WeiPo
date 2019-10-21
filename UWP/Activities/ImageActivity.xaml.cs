using System;
using System.Collections.Generic;
using System.IO;
using Windows.Foundation;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Flurl.Http;
using WeiPo.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeiPo.Activities
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ImageActivity
    {
        private bool _isPointerDown;
        private Point _prevPoint;

        public ImageActivity()
        {
            InitializeComponent();
        }

        public ImageViewModel ViewModel { get; set; }

        protected override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            if (parameter is ImageViewModel viewModel)
            {
                ViewModel = viewModel;
            }
        }

        private void ImageFlipView_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
        }

        private void UIElement_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                var position = e.GetPosition(scrollViewer);
                scrollViewer.ChangeView(null, null, scrollViewer.ZoomFactor + 1, false);
            }
        }

        private async void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var item = ViewModel.Images[ViewModel.SelectedIndex];
            var name = Path.GetFileName(item.Source);
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary, SuggestedFileName = name
            };
            picker.FileTypeChoices.Add("Image file", new List<string> { ".jpg", ".png", ".gif" });
            var file = await picker.PickSaveFileAsync();
            if (file == null)
            {
                return;
            }

            try
            {
                using var fstream = await file.OpenStreamForWriteAsync();
                using var stream = await item.Source.GetStreamAsync();
                await stream.CopyToAsync(fstream);
            }
            catch
            {
                //TODO: show notification
            }
        }
    }
}