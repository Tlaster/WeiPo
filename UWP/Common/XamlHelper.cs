using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WeiPo.Common
{
    class NonNullDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item != null)
            {
                return DataTemplate;
            }
            return new DataTemplate();
        }
    }

    static class XamlHelper
    {
        public static bool IsEqual(object thiz, object other)
        {
            return thiz == other;
        }

        public static Visibility IsEqualToVisibility(object thiz, object other)
        {
            return BoolToVisibility(IsEqual(thiz, other));
        }

        public static bool IsNonEqual(object thiz, object other)
        {
            return thiz == other;
        }

        public static Visibility IsNonEqualToVisibility(object thiz, object other)
        {
            return BoolToVisibility(IsNonEqual(thiz, other));
        }

        public static bool IsLongZero(long value)
        {
            return value == 0L;
        }

        public static bool IsLongNonZero(long value)
        {
            return value != 0L;
        }

        public static bool IsNull(object value)
        {
            return value == null;
        }

        public static bool IsNonNull(object value)
        {
            return value != null;
        }

        public static Visibility IsLongZeroToVisibility(long value)
        {
            return BoolToVisibility(IsLongZero(value));
        }

        public static Visibility IsLongNonZeroToVisibility(long value)
        {
            return BoolToVisibility(IsLongNonZero(value));
        }

        public static Visibility IsNullToVisibility(object value)
        {
            return BoolToVisibility(IsNull(value));
        }

        public static Visibility IsNonNullToVisibility(object value)
        {
            return BoolToVisibility(IsNonNull(value));
        }

        private static Visibility BoolToVisibility(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
