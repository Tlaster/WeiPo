using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Microsoft.UI.Xaml.Controls;

namespace WeiPo.Common
{
    public class StretchGridLayout : VirtualizingLayout
    {
        private class LayoutState
        {
            public int FirstIndex { get; set; } = -1;
            public int LastIndex { get; set; } = -1;

            public List<Rect> LayoutRects { get; set; } = new List<Rect>();
        }
        private int _firstRowIndex;
        private int _lastRowIndex;
        private double _itemSize;
        public double DesiredItemSize { get; set; } = 250D;

        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            base.InitializeForContextCore(context);
            context.LayoutState = new LayoutState();
        }

        protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
        {
            base.UninitializeForContextCore(context);
            context.LayoutState = null;
        }

        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            if (!(context.LayoutState is LayoutState state))
            {
                return Size.Empty;
            }
            var numColumns = (int) Math.Floor(availableSize.Width / Math.Min(DesiredItemSize, availableSize.Width));
            _itemSize = availableSize.Width / numColumns;
            var viewport = context.RealizationRect;

            _firstRowIndex = Math.Max(
                (int)(viewport.Y / DesiredItemSize) - 1,
                0);
            _lastRowIndex = Math.Min(
                (int)(viewport.Bottom / DesiredItemSize) + 1,
                (int)(context.ItemCount / numColumns));
            state.FirstIndex = _firstRowIndex * numColumns;
            for (int i = _firstRowIndex; i < _lastRowIndex; i++)
            {
                var firstItem = i * numColumns;
                var yoffset = i * _itemSize;
                for (int j = 0; j < numColumns; j++)
                {
                    var itemIndex = j + firstItem;
                    var item = context.GetOrCreateElementAt(itemIndex);
                    item.Measure(new Size(_itemSize, _itemSize));
                    if (itemIndex >= state.LayoutRects.Count) 
                    {
                        state.LayoutRects.Add(new Rect(j * this._itemSize, yoffset, _itemSize, _itemSize));
                    }
                    else if (state.LayoutRects[itemIndex].Width != _itemSize)
                    {
                        state.LayoutRects[itemIndex] = new Rect(j * this._itemSize, yoffset, _itemSize, _itemSize);
                    }
                    state.LastIndex = itemIndex;
                }
            }

            return new Size(_itemSize * numColumns, Math.Round(Convert.ToDouble(context.ItemCount) / Convert.ToDouble(numColumns)) * _itemSize);
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            if (!(context.LayoutState is LayoutState state))
            {
                return Size.Empty;
            }

            for (int i = state.FirstIndex; i < state.LastIndex; i++)
            {
                var container = context.GetOrCreateElementAt(i);
                container.Arrange(state.LayoutRects[i]);
            }

            return finalSize;
        }
    }
}