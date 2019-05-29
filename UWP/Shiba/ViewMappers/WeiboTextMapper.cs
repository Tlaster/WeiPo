using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Shiba.Controls;
using Shiba.ViewMappers;
using WeiPo.Common;
using WeiPo.Shiba.ViewMappers;

[assembly:ExportMapper("weiboText", typeof(WeiboTextMapper))]
namespace WeiPo.Shiba.ViewMappers
{
    public class WeiboTextMapper : ViewMapper<MarkdownTextBlock>
    {
        protected override MarkdownTextBlock CreateNativeView(IShibaContext context)
        {
            return base.CreateNativeView(context).Also(it =>
            {
                it.ImageMaxHeight = 14;
                it.Background = new SolidColorBrush(Colors.Transparent);
                it.TextWrapping = TextWrapping.Wrap;
            });
        }

        protected override IEnumerable<IValueMap> ExtendPropertyMap()
        {
            yield return new PropertyMap("text", MarkdownTextBlock.TextProperty, typeof(string), typeof(string));
        }
    }
}
