using System;
using System.Globalization;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Humanizer;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Uwp.UI.Controls;
using WeiPo.Common;
using WeiPo.Services.Models;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Controls
{

    internal class PageInfoDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is PageInfo pageInfo && (pageInfo.Type == "video" || pageInfo.Type == "article"))
            {
                return DataTemplate;
            }
            return new DataTemplate();
        }
    }

    internal static class StatusViewXamlHelper
    {
        public static Visibility PageInfoVisibility(PageInfo pageInfo)
        {
            return pageInfo != null && (pageInfo.Type == "video" || pageInfo.Type == "article")
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public static string TimeConverter(string time)
        {
            if (DateTime.TryParseExact(time, "ddd MMM dd HH:mm:ss K yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            {
                date = date.ToUniversalTime();
                return (DateTime.UtcNow - date).Hours > 3 ? date.ToString("f") : date.Humanize();
            }
            else
            {
                return time;
            }
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
            {
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
                }
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

        private void OnLinkClicked(object sender, WeiPo.Controls.Html.LinkClickedEventArgs e)
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
                Launcher.LaunchUriAsync(new Uri(e.Link));
            }
        }
    }
}