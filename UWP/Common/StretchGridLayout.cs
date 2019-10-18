using System;
using Windows.Foundation;
using Microsoft.UI.Xaml.Controls;

namespace WeiPo.Common
{
    public class StretchGridLayout : VirtualizingLayout
    {
        private int _firstRowIndex;
        private int _lastRowIndex;
        private double _itemSize;
        public double DesiredItemSize { get; set; } = 250D;

        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            var numColumns = (int) Math.Floor(availableSize.Width / Math.Min(DesiredItemSize, availableSize.Width));
            _itemSize = availableSize.Width / numColumns;
            var viewport = context.RealizationRect;

            _firstRowIndex = Math.Max(
                (int)(viewport.Y / DesiredItemSize) - 1,
                0);
            _lastRowIndex = Math.Min(
                (int)(viewport.Bottom / DesiredItemSize) + 1,
                (int)(context.ItemCount / numColumns));

            for (int i = _firstRowIndex; i < _lastRowIndex; i++)
            {
                var item = context.GetOrCreateElementAt(i);
                item.Measure(new Size(_itemSize, _itemSize));
            }

            return new Size(_itemSize * numColumns, Math.Round(Convert.ToDouble(context.ItemCount) / Convert.ToDouble(numColumns)) * _itemSize);
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            
            for (int i = _firstRowIndex; i < _lastRowIndex; i++)
            {
                var item = context.GetOrCreateElementAt(i);
            }
            return finalSize;
        }
    }
}