using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace WeiPo.Common
{
    internal static class Extensions
    {
        // Kotlin: fun <T> T.also(block: (T) -> Unit): T
        public static T Also<T>(this T self, Action<T> block)
        {
            block(self);
            return self;
        }

        public static void FireAndForget(this Task task)
        {
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static bool IsVisible(this UIElement element)
        {
            return element.Visibility == Visibility.Visible;
        }

        // Kotlin: fun <T, R> T.let(block: (T) -> R): R
        public static R Let<T, R>(this T self, Func<T, R> block)
        {
            return block(self);
        }

        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static async Task<StorageFile> SaveCacheFile(this IRandomAccessStreamReference reference,
            string name = null)
        {
            
            var file = await ApplicationData.Current.LocalCacheFolder.CreateFileAsync(name ?? $"{new Random().Next()}.jpg", CreationCollisionOption.GenerateUniqueName);
            using (var fstream = await file.OpenStreamForWriteAsync())
            {
                using var stream = await reference.OpenReadAsync();
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var pixels = await decoder.GetPixelDataAsync();
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fstream.AsRandomAccessStream());
                encoder.SetPixelData(decoder.BitmapPixelFormat, BitmapAlphaMode.Ignore,
                    decoder.OrientedPixelWidth, decoder.OrientedPixelHeight,
                    decoder.DpiX, decoder.DpiY,
                    pixels.DetachPixelData());
                await encoder.FlushAsync();
            }

            return file;
        }
    }
}