using System;
using Windows.System;
using WeiPo.ViewModels;

namespace WeiPo.Common
{
    internal static class WeiboLinkHelper
    {
        public static void LinkClicked(string link)
        {
            if (link.StartsWith("/n/"))
            {
                Singleton<MessagingCenter>.Instance.Send(null, "user_clicked", link.Substring("/n/".Length));
            }
            else if (link.StartsWith("/status/"))
            {
                Singleton<MessagingCenter>.Instance.Send(null, "status_clicked", link.Substring("/status/".Length));
            }
            else if (link.StartsWith("http"))
            {
                var uri = new Uri(link);
                if (uri.Host.Contains("sinaimg.cn"))
                {
                    Singleton<MessagingCenter>.Instance.Send(null, "image_clicked",
                        new ImageViewModel(new[] {new ImageModel(link, link)}));
                }
                else
                {
                    Launcher.LaunchUriAsync(new Uri(link));
                }
            }
        }
    }
}