using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls;

namespace WeiPo.Common
{
    public class NineGridPanel : Panel
    {
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding), typeof(double), typeof(NineGridLayout), new PropertyMetadata(8d));

        public double Padding
        {
            get => (double) GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var currentY = 0D;
            var currentX = 0D;
            var itemSize = finalSize.Width / 3D - Padding;
            foreach (var child in Children)
            {
                var bounds = new Rect(currentX, currentY, itemSize, itemSize);
                child.Arrange(bounds);

                currentX += itemSize + Padding;
                if (currentX >= finalSize.Width)
                {
                    currentX = 0D;
                    currentY += itemSize + Padding;
                }
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var itemSize = availableSize.Width / 3D - Padding;
            var rowCount = Math.Ceiling(Convert.ToDouble(Children.Count) / 3D);
            if (rowCount == 0d)
            {
                return new Size(availableSize.Width, 0d);
            }

            var totalHeight = rowCount * itemSize + (rowCount - 1) * Padding;
            for (var index = 0; index < Children.Count; index++)
            {
                var child = Children[index];
                child.Measure(new Size(itemSize, itemSize));
            }

            return new Size(availableSize.Width, totalHeight);
        }
    }

    public class NineGridLayout : VirtualizingLayout
    {
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding), typeof(double), typeof(NineGridLayout), new PropertyMetadata(8d));

        public double Padding
        {
            get => (double) GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            var currentY = 0D;
            var currentX = 0D;
            var itemSize = finalSize.Width / 3D - Padding;
            for (var i = 0; i < context.ItemCount; i++)
            {
                var child = context.GetOrCreateElementAt(i);
                var bounds = new Rect(currentX, currentY, itemSize, itemSize);
                child.Arrange(bounds);

                currentX += itemSize + Padding;
                if (currentX >= finalSize.Width)
                {
                    currentX = 0D;
                    currentY += itemSize + Padding;
                }
            }

            return finalSize;
        }

        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            var itemSize = availableSize.Width / 3D - Padding;
            var rowCount = Math.Ceiling(Convert.ToDouble(context.ItemCount) / 3D);
            if (rowCount == 0d)
            {
                return new Size(availableSize.Width, 0d);
            }

            var totalHeight = rowCount * itemSize + (rowCount - 1) * Padding;
            for (var i = 0; i < context.ItemCount; i++)
            {
                context.GetOrCreateElementAt(i).Measure(new Size(itemSize, itemSize));
            }

            return new Size(availableSize.Width, totalHeight);
        }
    }
}