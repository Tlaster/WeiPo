namespace WeiPo.Common
{
    internal static class StringExtensions
    {
        public static bool IsNonNullOrEmpty(this string value)
        {
            return !value.IsNullOrEmpty();
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
    }
}