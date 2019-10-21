using System;
using System.Collections.Generic;
using System.IO;
using Windows.Foundation;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Flurl.Http;
using Microsoft.Toolkit.Uwp.UI.Controls;
using WeiPo.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeiPo.Activities
{
    public class ImageEx2 : ImageEx
    {
        public static readonly DependencyProperty ViewportHeightProperty = DependencyProperty.Register(
            "ViewportHeight", typeof(double), typeof(ImageEx2),
            new PropertyMetadata(default(double), PropertyChangedCallback));

        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register(
            "ImageHeight", typeof(double), typeof(ImageEx2),
            new PropertyMetadata(default(double), PropertyChangedCallback));

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register(
            "ImageWidth", typeof(double), typeof(ImageEx2),
            new PropertyMetadata(default(double), PropertyChangedCallback));


        public double ViewportHeight
        {
            get => (double) GetValue(ViewportHeightProperty);
            set => SetValue(ViewportHeightProperty, value);
        }

        public double ImageHeight
        {
            get => (double) GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public double ImageWidth
        {
            get => (double) GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }


        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == ImageHeightProperty || e.Property == ImageWidthProperty ||
                e.Property == ViewportHeightProperty)
            {
                (d as ImageEx2).OnSizeChanged();
            }
        }

        private void OnSizeChanged()
        {
            if (ImageWidth == 0d || ImageHeight == 0d || ViewportHeight == 0d)
            {
                return;
            }

            if (ImageHeight > ImageWidth * 3)
            {
                //long image
            }
            else
            {
                Height = ViewportHeight;
            }
        }
    }

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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = ViewModel.Images[ViewModel.SelectedIndex];
            var name = Path.GetFileName(item.Source);
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary, SuggestedFileName = name
            };
            picker.FileTypeChoices.Add("Image file", new List<string> {".jpg", ".png", ".gif"});
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