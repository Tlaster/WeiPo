using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace WeiPo.Common
{
    public class StaggeredLayout : VirtualizingLayout
    {
        private class State
        {
            public Dictionary<int, Size> ChildSize { get; } = new Dictionary<int, Size>();
            public Dictionary<int, Rect> ArrangeData { get; } = new Dictionary<int, Rect>();
        }
        public static readonly DependencyProperty DesiredColumnWidthProperty = DependencyProperty.Register(
            nameof(DesiredColumnWidth), typeof(double), typeof(StaggeredLayout), new PropertyMetadata(250D));

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register(
            nameof(Padding),
            typeof(Thickness),
            typeof(StaggeredLayout),
            new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty VerticalOffsetProperty = DependencyProperty.Register(
            nameof(VerticalOffset), typeof(double), typeof(StaggeredLayout), new PropertyMetadata(default(double)));

        private double _columnWidth;

        public double VerticalOffset
        {
            get => (double) GetValue(VerticalOffsetProperty);
            set => SetValue(VerticalOffsetProperty, value);
        }

        public Thickness Padding
        {
            get => (Thickness) GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }

        public double DesiredColumnWidth
        {
            get => (double) GetValue(DesiredColumnWidthProperty);
            set => SetValue(DesiredColumnWidthProperty, value);
        }

        protected override void InitializeForContextCore(VirtualizingLayoutContext context)
        {
            base.InitializeForContextCore(context);
            context.LayoutState = new State();
        }

        protected override void UninitializeForContextCore(VirtualizingLayoutContext context)
        {
            base.UninitializeForContextCore(context);
        }

        protected override Size ArrangeOverride(VirtualizingLayoutContext context, Size finalSize)
        {
            var state = context.LayoutState as State;
            foreach (var item in state.ArrangeData)
            {
                var child = context.GetOrCreateElementAt(item.Key);
                child.Arrange(item.Value);
            }
            return finalSize;
        }

        protected override Size MeasureOverride(VirtualizingLayoutContext context, Size availableSize)
        {
            var state = context.LayoutState as State;
            availableSize.Width = availableSize.Width - Padding.Left - Padding.Right;
            availableSize.Height = availableSize.Height - Padding.Top - Padding.Bottom - VerticalOffset;
            _columnWidth = Math.Min(DesiredColumnWidth, availableSize.Width);
            var numColumns = (int) Math.Floor(availableSize.Width / _columnWidth);
            _columnWidth = availableSize.Width / numColumns;
            var columnHeights = new double[numColumns];
            var verticalOffset = Padding.Top + VerticalOffset;
            var horizontalOffset = Padding.Left;
            horizontalOffset += (availableSize.Width - numColumns * _columnWidth) / 2;
            state.ArrangeData.Clear();
            for (var i = 0; i < context.ItemCount; i++)
            {
                var columnIndex = GetColumnIndex(columnHeights);
                var realizationRect = context.RealizationRect;
                UIElement child = null;
                if (!state.ChildSize.ContainsKey(i) || state.ChildSize[i].Width != _columnWidth)
                {
                    child = context.GetOrCreateElementAt(i);
                    child.Measure(new Size(_columnWidth, availableSize.Height));
                    if (!state.ChildSize.ContainsKey(i))
                    {
                        state.ChildSize.Add(i, child.DesiredSize);
                    }
                    else
                    {
                        state.ChildSize[i] = child.DesiredSize;
                    }
                    var bounds = new Rect(horizontalOffset + _columnWidth * columnIndex,
                        columnHeights[columnIndex] + verticalOffset, _columnWidth, child.DesiredSize.Height);
                    state.ArrangeData.Add(i, bounds);
                }

                if (columnHeights[columnIndex] < realizationRect.Bottom && columnHeights[columnIndex] + state.ChildSize[i].Height > realizationRect.Top && child == null)
                {
                    child = context.GetOrCreateElementAt(i);
                    child.Measure(new Size(_columnWidth, availableSize.Height));
                    if (state.ChildSize[i] != child.DesiredSize)
                    {
                        state.ChildSize[i] = child.DesiredSize;
                    }
                    var bounds = new Rect(horizontalOffset + _columnWidth * columnIndex,
                        columnHeights[columnIndex] + verticalOffset, _columnWidth, child.DesiredSize.Height);
                    state.ArrangeData.Add(i, bounds);
                }
                else if (child != null)
                {
                    context.RecycleElement(child);
                }
                
                var elementSize = state.ChildSize[i];
                columnHeights[columnIndex] += elementSize.Height;
            }

            var desiredHeight = columnHeights.Max();

            return new Size(availableSize.Width, desiredHeight);
        }

        private int GetColumnIndex(double[] columnHeights)
        {
            var columnIndex = 0;
            var height = columnHeights[0];
            for (var j = 1; j < columnHeights.Length; j++)
            {
                if (columnHeights[j] < height)
                {
                    columnIndex = j;
                    height = columnHeights[j];
                }
            }

            return columnIndex;
        }
    }
}