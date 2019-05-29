using System;
using Windows.Foundation;
using Microsoft.UI.Xaml.Controls;

namespace WeiPo.Common
{
    public class NineGridLayout : VirtualizingLayout
    {
        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            var itemSize = availableSize.Width / 3D;
            var totalHeight = Math.Ceiling(Convert.ToDouble(context.ItemCount) / 3D) * itemSize;
            for (var i = 0; i < context.ItemCount; i++)
                context.GetOrCreateElementAt(i).Measure(new Size(itemSize, itemSize));

            return new Size(availableSize.Width, totalHeight);
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            var currentY = 0D;
            var currentX = 0D;
            var itemSize = finalSize.Width / 3D;
            for (var i = 0; i < context.ItemCount; i++)
            {
                var child = context.GetOrCreateElementAt(i);
                var bounds = new Rect(currentX, currentY, itemSize, itemSize);
                child.Arrange(bounds);

                currentX += itemSize;
                if (currentX >= finalSize.Width)
                {
                    currentX = 0D;
                    currentY += itemSize;
                }
            }

            return finalSize;
        }
    }
}