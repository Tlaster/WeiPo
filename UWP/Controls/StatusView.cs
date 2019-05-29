using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Parsers.Markdown;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;
using Microsoft.Toolkit.Parsers.Markdown.Render;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Render;
using Newtonsoft.Json.Linq;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace WeiPo.Controls
{
    internal class WeiboVideoViewSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is JObject obj)
            {
                if (obj.ContainsKey("page_info") && obj["page_info"]["type"].Value<string>() == "video")
                {
                    return DataTemplate;
                }
            }
            return new DataTemplate();
        }
    }

    internal class WeiboImageViewSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is JObject obj)
            {
                if (obj.ContainsKey("pics"))
                {
                    return DataTemplate;
                }
            }
            return new DataTemplate();
        }
    }

    internal class RetweetStatusSelector : DataTemplateSelector
    {
        public DataTemplate DataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is JObject obj)
            {
                if (obj.ContainsKey("retweeted_status"))
                {
                    return DataTemplate;
                }
            }
            return new DataTemplate();
        }
    }

    public sealed class StatusView : Control
    {
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            nameof(Status), typeof(JObject), typeof(StatusView), new PropertyMetadata(default(JObject)));

        public StatusView()
        {
            DefaultStyleKey = typeof(StatusView);
        }

        public JObject Status
        {
            get => (JObject) GetValue(StatusProperty);
            set => SetValue(StatusProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}