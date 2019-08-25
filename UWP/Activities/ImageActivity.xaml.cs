using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
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
            if (parameter is ImageViewModel viewModel) ViewModel = viewModel;
        }

        private void ImageFlipView_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
        }

        private void UIElement_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                var position = e.GetPosition(scrollViewer);
                scrollViewer.ZoomBy(1F, new Vector2(Convert.ToSingle(position.X), Convert.ToSingle(position.Y)),
                    new ZoomOptions(AnimationMode.Enabled));
            }
        }
    }
}