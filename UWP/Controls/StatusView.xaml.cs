using System;
using System.Globalization;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Humanizer;
using WeiPo.Common;
using WeiPo.Controls.Html;
using WeiPo.Services.Models;
using WeiPo.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Controls
{
    internal class PageInfoDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is PageInfo pageInfo && (pageInfo.Type == "video" || pageInfo.Type == "article"))
                return DataTemplate;
            return new DataTemplate();
        }
    }

    internal static class StatusViewXamlHelper
    {
        public static string SecondToTime(double second)
        {
            var time = new TimeSpan(0, 0, Convert.ToInt32(second));
            return time.ToString("g");
        }

        public static Visibility PageInfoVisibility(StatusModel status)
        {
            return status.PageInfo != null && (status.PageInfo.Type == "video" || status.PageInfo.Type == "article")
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public static string TimeConverter(string time)
        {
            if (DateTime.TryParseExact(time, "ddd MMM dd HH:mm:ss K yyyy", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var date))
            {
                date = date.ToUniversalTime();
                return (DateTime.UtcNow - date).Hours > 3 ? date.ToString("f") : date.Humanize();
            }

            return time;
        }
    }

    public sealed partial class StatusView : UserControl
    {
        public static readonly DependencyProperty ShowActionsProperty = DependencyProperty.Register(
            nameof(ShowActions), typeof(bool), typeof(StatusView), new PropertyMetadata(true));

        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            nameof(Status), typeof(StatusModel), typeof(StatusView), new PropertyMetadata(default(StatusModel)));

        public StatusView()
        {
            InitializeComponent();
        }

        public bool ShowActions
        {
            get => (bool) GetValue(ShowActionsProperty);
            set => SetValue(ShowActionsProperty, value);
        }

        public StatusModel Status
        {
            get => (StatusModel) GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            if (e.OriginalSource is FrameworkElement element && element.DataContext != null)
                switch (element.DataContext)
                {
                    case StatusModel status:
                        e.Handled = true;
                        Singleton<MessagingCenter>.Instance.Send(this, "status_clicked", status);
                        break;
                    case UserModel user:
                        e.Handled = true;
                        Singleton<MessagingCenter>.Instance.Send(this, "user_clicked", user);
                        break;
                    case PageInfo info:
                        switch (info.Type)
                        {
                            case "video":
                            {
                                e.Handled = true;
                                //trick: new [] { "mp4_720p_mp4", "mp4_hd_mp4", "mp4_ld_mp4", "mp4_1080p_mp4", "pre_ld_mp4"}.OrderBy(it => it) => OrderedEnumerable<string, string> { "mp4_1080p_mp4", "mp4_720p_mp4", "mp4_hd_mp4", "mp4_ld_mp4", "pre_ld_mp4" }
                                var url = info.Urls?.OrderBy(it => it.Key).FirstOrDefault().Value ??
                                          info.MediaInfo.StreamUrlHd;
                                Singleton<MessagingCenter>.Instance.Send(this, "video_clicked", url);
                            }
                                break;
                            case "article":
                            {
                                if (info.PageUrl != null) Launcher.LaunchUriAsync(new Uri(info.PageUrl));
                            }
                                break;
                        }

                        break;
                }
        }

        protected override void OnRightTapped(RightTappedRoutedEventArgs e)
        {
            base.OnRightTapped(e);
        }

        private void ShareTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Singleton<MessagingCenter>.Instance.Send(this, "status_share", Status);
        }

        private void CommentTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Singleton<MessagingCenter>.Instance.Send(this, "status_comment", Status);
        }

        private void LikeTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Singleton<MessagingCenter>.Instance.Send(this, "status_like", Status);
        }

        private void OnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (e.Link.StartsWith("/n/"))
            {
                Singleton<MessagingCenter>.Instance.Send(this, "user_clicked", e.Link.Substring("/n/".Length));
            }
            else if (e.Link.StartsWith("/status/"))
            {
                Singleton<MessagingCenter>.Instance.Send(this, "status_clicked", e.Link.Substring("/status/".Length));
            }
            else if (e.Link.StartsWith("http"))
            {
                var uri = new Uri(e.Link);
                if (uri.Host.Contains("sinaimg.cn"))
                    Singleton<MessagingCenter>.Instance.Send(this, "image_clicked",
                        new ImageViewModel(new[] {new ImageModel(e.Link, e.Link)}));
                else
                    Launcher.LaunchUriAsync(new Uri(e.Link));
            }
        }

        private void ImageListOnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && element.DataContext is Pic pic && Status.Pics != null &&
                Status.Pics.Contains(pic))
            {
                var index = Status.Pics.IndexOf(pic);
                Singleton<MessagingCenter>.Instance.Send(this, "image_clicked",
                    new ImageViewModel(
                        Status.Pics.Select(it =>
                            new ImageModel(it.Url, it.Large.Url, it.Large.Geo.Width, it.Large.Geo.Height)).ToArray(),
                        index));
            }
        }
    }
}