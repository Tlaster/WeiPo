using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WeiPo.Controls
{
    internal class AspectRatioView : Panel
    {
        public static readonly DependencyProperty WidthRequestProperty = DependencyProperty.Register(
            nameof(WidthRequest), typeof(int), typeof(AspectRatioView), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty HeightRequestProperty = DependencyProperty.Register(
            nameof(HeightRequest), typeof(int), typeof(AspectRatioView), new PropertyMetadata(default(int)));

        public int WidthRequest
        {
            get => (int) GetValue(WidthRequestProperty);
            set => SetValue(WidthRequestProperty, value);
        }

        public int HeightRequest
        {
            get => (int) GetValue(HeightRequestProperty);
            set => SetValue(HeightRequestProperty, value);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var availableWidth = availableSize.Width;
            var requestHeight = Convert.ToDouble(HeightRequest) / Convert.ToDouble(WidthRequest) * Convert.ToDouble(availableWidth);
            var size = new Size(availableWidth, requestHeight);
            foreach (var item in this.Children)
            {
                item.Measure(size);
            }

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var availableWidth = finalSize.Width;
            var requestHeight = Convert.ToDouble(HeightRequest) / Convert.ToDouble(WidthRequest) * Convert.ToDouble(availableWidth);
            var size = new Size(availableWidth, requestHeight);
            var rect = new Rect(0, 0, size.Width, size.Height);
            foreach (var item in this.Children)
            {
                item.Arrange(rect);
            }

            return size;
        }
    }
}