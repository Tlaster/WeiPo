using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Media.Streaming.Adaptive;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace WeiPo.Common
{
    internal static class Extensions
    {
        public static bool IsVisible(this UIElement element)
        {
            return element.Visibility == Visibility.Visible;
        }

        public static void FireAndForget(this Task task)
        {

        }

        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        // Kotlin: fun <T, R> T.let(block: (T) -> R): R
        public static R Let<T, R>(this T self, Func<T, R> block)
        {
            return block(self);
        }

        // Kotlin: fun <T> T.also(block: (T) -> Unit): T
        public static T Also<T>(this T self, Action<T> block)
        {
            block(self);
            return self;
        }
    }
}
