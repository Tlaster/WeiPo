using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using HtmlAgilityPack;
using Microsoft.Toolkit.Uwp.UI.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace WeiPo.Controls.Html
{
    public interface IHtmlRenderContext
    {
        InlineCollection InlineCollection { get; }
    }

    public class RenderContext : IHtmlRenderContext
    {
        public RenderContext(InlineCollection inlineCollection)
        {
            InlineCollection = inlineCollection;
        }

        public InlineCollection InlineCollection { get; }
    }

    public class LinkClickedEventArgs
    {
        public LinkClickedEventArgs(string link)
        {
            Link = link;
        }

        public string Link { get; }
    }

    public sealed class HtmlTextBlock : Control
    {
        public static readonly DependencyProperty HyperlinkUrlProperty = DependencyProperty.Register(
            nameof(HyperlinkUrl), typeof(string), typeof(HtmlTextBlock), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(HtmlTextBlock), new PropertyMetadata(default, OnPropertyChanged));

        public static readonly DependencyProperty IsTextSelectionEnabledProperty = DependencyProperty.Register(
            nameof(IsTextSelectionEnabled), typeof(bool), typeof(HtmlTextBlock), new PropertyMetadata(default));

        private readonly List<Hyperlink> _listeningHyperlinks = new List<Hyperlink>();
        private RichTextBlock _richTextContent;
        private Border _rootElement;

        public HtmlTextBlock()
        {
            DefaultStyleKey = typeof(HtmlTextBlock);
        }

        public bool IsTextSelectionEnabled
        {
            get => (bool) GetValue(IsTextSelectionEnabledProperty);
            set => SetValue(IsTextSelectionEnabledProperty, value);
        }

        public string HyperlinkUrl
        {
            get => (string) GetValue(HyperlinkUrlProperty);
            set => SetValue(HyperlinkUrlProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public event EventHandler<LinkClickedEventArgs> LinkClicked;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rootElement = GetTemplateChild("RootElement") as Border;
            _richTextContent = GetTemplateChild("RichTextContent") as RichTextBlock;
            RenderHtml();
        }

        private void Clean()
        {
            _listeningHyperlinks.ForEach(it => it.Click -= HyperLinkOnClick);
            _listeningHyperlinks.Clear();
        }

        private void HyperLinkOnClick(Hyperlink sender, HyperlinkClickEventArgs args)
        {
            LinkClicked?.Invoke(this, new LinkClickedEventArgs(sender.GetValue(HyperlinkUrlProperty) as string));
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == TextProperty)
            {
                (d as HtmlTextBlock).RenderHtml();
            } 
        }

        private void Render(HtmlNode node, IHtmlRenderContext context)
        {
            switch (node.Name.TrimStart('#'))
            {
                case "a":
                    RenderA(node, context);
                    break;
                case "span":
                    RenderSpan(node, context);
                    break;
                case "img":
                    RenderImg(node, context);
                    break;
                case "br":
                    RenderBr(node, context);
                    break;
                case "text":
                    context.InlineCollection.Add(new Run {Text = node.InnerText});
                    break;
                default:
                    if (node.HasChildNodes)
                    {
                        Render(node.ChildNodes, context);
                    }

                    break;
            }
        }

        private void Render(HtmlNodeCollection nodeChildNodes, IHtmlRenderContext context)
        {
            foreach (var item in nodeChildNodes)
            {
                Render(item, context);
            }
        }

        private void RenderA(HtmlNode node, IHtmlRenderContext context)
        {
            var link = node.GetAttributeValue("href", "");
            var hyperLink = new Hyperlink();
            Render(node.ChildNodes, new RenderContext(hyperLink.Inlines));
            hyperLink.SetValue(HyperlinkUrlProperty, link);
            ToolTipService.SetToolTip(hyperLink, link);
            hyperLink.Click += HyperLinkOnClick;
            _listeningHyperlinks.Add(hyperLink);
            context.InlineCollection.Add(hyperLink);
        }

        private void RenderBr(HtmlNode node, IHtmlRenderContext context)
        {
            context.InlineCollection.Add(new LineBreak());
        }

        private void RenderHtml()
        {
            if (_rootElement == null || _richTextContent == null)
            {
                return;
            }

            Clean();

            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            var html = new HtmlDocument();
            html.LoadHtml(Text);
            _richTextContent.Blocks.Clear();
            var paragraph = new Paragraph();
            _richTextContent.Blocks.Add(paragraph);
            Render(html.DocumentNode, new RenderContext(paragraph.Inlines));
        }

        private void RenderIconImg(HtmlNode node, IHtmlRenderContext context)
        {
            if (node.HasAttributes && node.Attributes.Any(it => it.Name == "alt"))
            {
                var name = node.GetAttributeValue("alt", "").TrimStart('[').TrimEnd(']');
                var link = node.GetAttributeValue("src", "");
                var img = new ImageEx
                {
                    Source = $"https:{link}",
                    Width = _richTextContent.FontSize,
                    Height = _richTextContent.FontSize,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.UniformToFill
                };
                var container = new InlineUIContainer {Child = img};
                ToolTipService.SetToolTip(img, name);
                context.InlineCollection.Add(container);
            }
        }

        private void RenderImg(HtmlNode node, IHtmlRenderContext context)
        {
            if (node.HasAttributes && node.Attributes.Any(it => it.Name == "alt"))
            {
                var name = node.GetAttributeValue("alt", "").TrimStart('[').TrimEnd(']');
                var link = node.GetAttributeValue("src", "");
                var img = new ImageEx
                {
                    Width = _richTextContent.FontSize,
                    Height = _richTextContent.FontSize,
                    Source = link,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Stretch = Stretch.UniformToFill
                };
                var container = new InlineUIContainer {Child = img};
                ToolTipService.SetToolTip(img, name);
                context.InlineCollection.Add(container);
            }
        }


        private void RenderSpan(HtmlNode node, IHtmlRenderContext context)
        {
            if (node.HasAttributes)
            {
                if (node.Attributes.Any(it => it.Name == "class" && it.Value == "url-icon")) // icon
                {
                    RenderIconImg(node.FirstChild, context);
                }
            }

            context.InlineCollection.Add(new Run {Text = node.InnerText});
        }
    }
}