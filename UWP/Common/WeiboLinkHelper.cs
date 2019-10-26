using System;
using System.Threading.Tasks;
using Windows.System;
using Flurl.Http;
using HtmlAgilityPack;
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
                else if (uri.Host.Contains("photo.weibo.com"))
                {
                    GetImageFromWeb(link);
                }
                else
                {
                    Launcher.LaunchUriAsync(new Uri(link));
                }
            }
        }

        private static async Task GetImageFromWeb(string link)
        {
            var html = await link.GetStringAsync();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode.SelectSingleNode("//img");
            if (node != null && node.HasAttributes && !string.IsNullOrEmpty(node.GetAttributeValue("src", string.Empty)))
            {
                var src = node.GetAttributeValue("src", "");
                Singleton<MessagingCenter>.Instance.Send(null, "image_clicked",
                    new ImageViewModel(new[] {new ImageModel(src, src)}));
            }
            else
            {
                Launcher.LaunchUriAsync(new Uri(link));
            }
        }
    }
}