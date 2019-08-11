using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace WeiPo.Common
{
    class WeiboHtmlToMarkdown
    {
        public static WeiboHtmlToMarkdown Instance { get; } = new WeiboHtmlToMarkdown();
        public string Convert(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var root = doc.DocumentNode;
            return Visit(root);
        }

        private string Visit(HtmlNode node)
        {
            switch (node.Name.TrimStart('#'))
            {
                case "a":
                    return VisitA(node);
                case "span":
                    return VisitSpan(node);
                case "img":
                    return VisitImg(node);
                case "br":
                    return Environment.NewLine;
                case "text":
                    return node.InnerText;
                default:
                {
                    return !node.HasChildNodes
                        ? string.Empty
                        : Visit(node.ChildNodes);
                }
            }
        }

        private string Visit(HtmlNodeCollection nodeChildNodes)
        {
            return nodeChildNodes.Aggregate(string.Empty, (current, nd) => current + Visit(nd));
        }

        private string VisitImg(HtmlNode node)
        {
            if (node.HasAttributes && node.Attributes.Any(it => it.Name == "alt"))
            {
                var name = node.GetAttributeValue("alt", "").TrimStart('[').TrimEnd(']');
                var link = node.GetAttributeValue("src", "");
                return $"![{name}]({link})";
            }

            return string.Empty;
        }

        private string VisitA(HtmlNode node)
        {
            var link = node.GetAttributeValue("href", "");
            var content = Visit(node.ChildNodes);
            return $"[{content}]({link})";
        }


        private string VisitSpan(HtmlNode node)
        {
            if (node.HasAttributes)
            {
                if (node.Attributes.Any(it => it.Name == "class" && it.Value == "url-icon")) // icon
                {
                    return VisitIconImg(node.FirstChild);
                }
            }

            return node.InnerText;
        }

        private string VisitIconImg(HtmlNode node)
        {
            if (node.HasAttributes && node.Attributes.Any(it => it.Name == "alt"))
            {
                var name = node.GetAttributeValue("alt", "").TrimStart('[').TrimEnd(']');
                var link = node.GetAttributeValue("src", "");
                return $"![{name}](https:{link})";
            }

            return string.Empty;
        }
    }
}