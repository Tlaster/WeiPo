using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Controls;
using Shiba;
using Shiba.Controls;
using Shiba.ViewMappers;
using WeiPo.Common;
using WeiPo.Shiba.ViewMappers;

[assembly:ExportMapper("items", typeof(ItemsMapper))]
namespace WeiPo.Shiba.ViewMappers
{
    public class ItemsMapper : ViewMapper<ItemsRepeater>
    {
        protected override ItemsRepeater CreateNativeView(IShibaContext context)
        {
            return new ItemsRepeater().Also(it =>
            {
                it.SetBinding(ItemsRepeater.DataContextProperty, new Binding
                {
                    Source = context.ShibaHost,
                    Path = new PropertyPath("DataContext"),
                });
            });
        }

        protected override IEnumerable<IValueMap> ExtendPropertyMap()
        {
            yield return new PropertyMap("source", ItemsRepeater.ItemsSourceProperty, typeof(IEnumerable));
            yield return new ManuallyValueMap("layout", typeof(string), (element, o) =>
            {
                if (element is ItemsRepeater itemsRepeater && o is string value)
                {
                    switch (value)
                    {
                        case "nineGrid":
                            itemsRepeater.Layout = new NineGridLayout();
                            break;
                    }
                }
            });
            yield return new ManuallyValueMap("creator", typeof(string), (element, o) =>
            {
                if (element is ItemsRepeater itemsRepeater && o is string name)
                {
                    var template = (DataTemplate)XamlReader.Load(
                        $"<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><views:{typeof(ShibaHost).Name} xmlns:views=\"using:{typeof(ShibaHost).Namespace}\" DataContext=\"{{Binding}}\" Creator=\"{name}\"/></DataTemplate>");
                    itemsRepeater.ItemTemplate = template;
                }
            });
        }
    }
}
