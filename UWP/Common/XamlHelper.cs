using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WeiPo.Common
{
    internal class NonNullDataTemplateSelector : DataTemplateSelector
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

    internal static class XamlHelper
    {
        public static bool IsEqual(object thiz, object other)
        {
            return thiz == other;
        }

        public static Visibility IsEqualToVisibility(object thiz, object other)
        {
            return BoolToVisibility(IsEqual(thiz, other));
        }

        public static bool IsIntEqual(int thiz, int other)
        {
            return thiz == other;
        }

        public static Visibility IsLongGreaterThanToVisibility(long thiz, long other)
        {
            return BoolToVisibility(thiz > other);
        }

        public static Visibility IsIntEqualToVisibility(int thiz, int other)
        {
            return BoolToVisibility(thiz == other);
        }

        public static bool IsIntNonEqual(int thiz, int other)
        {
            return thiz != other;
        }

        public static Visibility IsIntNonEqualToVisibility(int thiz, int other)
        {
            return BoolToVisibility(thiz != other);
        }

        public static Visibility IsLongEqualToVisibility(long thiz, long other)
        {
            return BoolToVisibility(thiz == other);
        }

        public static Visibility IsLongNonEqualToVisibility(long thiz, long other)
        {
            return BoolToVisibility(thiz != other);
        }

        public static bool IsLongNonZero(long value)
        {
            return value != 0L;
        }

        public static Visibility IsLongNonZeroToVisibility(long value)
        {
            return BoolToVisibility(IsLongNonZero(value));
        }

        public static bool IsLongZero(long value)
        {
            return value == 0L;
        }

        public static Visibility IsLongZeroToVisibility(long value)
        {
            return BoolToVisibility(IsLongZero(value));
        }

        public static bool IsNonEqual(object thiz, object other)
        {
            return thiz == other;
        }

        public static Visibility IsNonEqualToVisibility(object thiz, object other)
        {
            return BoolToVisibility(IsNonEqual(thiz, other));
        }

        public static bool IsNonNull(object value)
        {
            return value != null;
        }

        public static Visibility IsNonNullToVisibility(object value)
        {
            return BoolToVisibility(IsNonNull(value));
        }

        public static bool IsNull(object value)
        {
            return value == null;
        }

        public static Visibility IsNullToVisibility(object value)
        {
            return BoolToVisibility(IsNull(value));
        }

        private static Visibility BoolToVisibility(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }

        public static Visibility InvertBoolToVisibility(bool value)
        {
            return value ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}