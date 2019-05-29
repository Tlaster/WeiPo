using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Shiba.Controls;
using Shiba.ViewMappers;
using WeiPo.Common;
using WeiPo.Shiba.ViewMappers;

[assembly:ExportMapper("img", typeof(ImgMapper))]
[assembly:ExportMapper("roundImg", typeof(RoundImgMapper))]
namespace WeiPo.Shiba.ViewMappers
{
    public class ImgMapper : ViewMapper<ImageEx>
    {
        protected override ImageEx CreateNativeView(IShibaContext context)
        {
            return base.CreateNativeView(context).Also(it => it.Stretch = Stretch.UniformToFill);
        }

        protected override IEnumerable<IValueMap> ExtendPropertyMap()
        {
            yield return new PropertyMap("source", ImageEx.SourceProperty, typeof(string));
        }
    }

    public class RoundImgMapper : ImgMapper
    {
        protected override ImageEx CreateNativeView(IShibaContext context)
        {
            return base.CreateNativeView(context).Also(it => it.CornerRadius = new CornerRadius(999));
        }
    }
}
