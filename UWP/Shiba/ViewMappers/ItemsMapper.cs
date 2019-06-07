using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email.DataProvider;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.UI.Xaml.Controls;
using Shiba;
using Shiba.Controls;
using Shiba.ViewMappers;
using WeiPo.Common;
using WeiPo.Shiba.ViewMappers;

[assembly:ExportMapper("items", typeof(ItemsMapper))]
namespace WeiPo.Shiba.ViewMappers
{
    internal class ItemsDataSource : IIncrementalSource<object>
    {
        private readonly List<object> _items = new List<object>();
        private const string FunctionName = "getPagedItemsAsync";

        public ItemsDataSource(object creatorInstance)
        {
            CreatorInstance = creatorInstance;
        }
        
        public object CreatorInstance { get; }
        public async Task<IEnumerable<object>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            if (pageIndex == 0)
            {
                _items.ForEach(it => { ShibaApp.Instance.Configuration.ScriptRuntime.ReleaseObj(it); });
                _items.Clear();
            }
            var scriptResult = ShibaApp.Instance.Configuration.ScriptRuntime.CallFunction(CreatorInstance, FunctionName,
                pageIndex);
            if (scriptResult is Task<object> task)
            {
                var result = await task;

                if (ShibaApp.Instance.Configuration.ScriptRuntime.IsArray(result))
                {
                    var items = ShibaApp.Instance.Configuration.ScriptRuntime.ToArray(result).Cast<object>().ToList();
                    foreach (var item in items)
                    {
                        ShibaApp.Instance.Configuration.ScriptRuntime.AddRef(item);
                        _items.Add(item);
                    }

                    return items;
                }
            }

            return new object[0]; 
        }
    }

    //internal class ItemsSourceCollection : IncrementalLoadingCollection<>

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
            yield return new ManuallyValueMap("source", typeof(object), (element, o) =>
            {
                if (element is ItemsRepeater repeater)
                {
                    switch (o)
                    {
                        case string sourceCreator:
                            repeater.ItemsSource = new IncrementalLoadingCollection<ItemsDataSource, object>(new ItemsDataSource(ShibaApp.Instance.Configuration.ScriptRuntime.CallFunction(sourceCreator)));
                            break;
                        case BindingBase binding:
                            repeater.SetBinding(ItemsRepeater.ItemsSourceProperty, binding);
                            break;
                        case IEnumerable enumerable:
                            repeater.ItemsSource = enumerable;
                            break;
                    }
                }
            });
            yield return new ManuallyValueMap("layout", typeof(string), (element, o) =>
            {
                if (element is ItemsRepeater itemsRepeater && o is string value)
                {
                    switch (value)
                    {
                        case "nineGrid":
                            itemsRepeater.Layout = new NineGridLayout();
                            break;
                        case "staggered":
                            itemsRepeater.Layout = new StaggeredLayout();
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

        protected override ItemsRepeater Map(View view, IShibaContext context)
        {
            var result = base.Map(view, context);
            if (result.ItemsSource is IncrementalLoadingCollection<ItemsDataSource, object> source)
            {
                source.RefreshAsync().FireAndForget();
            }
            return result;
        }
    }
}
