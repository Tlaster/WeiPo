using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Microsoft.Toolkit.Uwp.Helpers;
using Shiba.Controls;
using Shiba.Internal;
using Shiba.ViewMappers;
using WeiPo.Common;

[assembly: ExportMapper("optional", typeof(OptionalAllowChildViewMapper))]
namespace Shiba.ViewMappers
{

    internal class OptionalViewSelector : DataTemplateSelector
    {
        private DataTemplate _dataTemplate;

        public DataTemplate DataTemplate
        {
            get
            {
                if (_dataTemplate == null)
                {
                    _dataTemplate = (DataTemplate) XamlReader.Load(
                        $"<DataTemplate xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><views:{typeof(ShibaHost).Name} xmlns:views=\"using:{typeof(ShibaHost).Namespace}\" DataContext=\"{{Binding}}\" Creator=\"{FunctionName}\"/></DataTemplate>");
                }

                return _dataTemplate;
            }
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item == null || Converter == null)
            {
                return new DataTemplate();
            }
            var result = ShibaApp.Instance.Configuration.ScriptRuntime.CallFunction(Converter, item);
            if (result is bool isShown && isShown)
            {
                return DataTemplate;
            }
            return new DataTemplate();
        }

        public string FunctionName { get; set; }
        public string Converter { get; set; }
    }

    public class OptionalAllowChildViewMapper : ViewMapper<ContentControl>
    {
        protected override ContentControl CreateNativeView(IShibaContext context)
        {
            return new ContentControl().Also(it =>
            {
                it.SetBinding(ContentControl.ContentProperty, new Binding
                {
                    Source = context,
                    Path = new PropertyPath(nameof(context.DataContext))
                });
            });
        }

        protected override IEnumerable<IValueMap> ExtendPropertyMap()
        {
            yield return new ManuallyValueMap("when", typeof(string), (element, o) =>
            {
                if (element is ContentControl view && o is string value)
                {
                    if (view.Tag == null)
                    {
                        view.Tag = new OptionalViewSelector();
                    }
                    view.Tag.Let(it => it as OptionalViewSelector).Also(it =>
                    {
                        it.Converter = value;
                    });
                }
            });
            yield return new ManuallyValueMap("show", typeof(string), (element, o) =>
            {
                if (element is ContentControl view && o is string value)
                {
                    if (view.Tag == null)
                    {
                        view.Tag = new OptionalViewSelector();
                    }
                    view.Tag.Let(it => it as OptionalViewSelector).Also(it =>
                    {
                        it.FunctionName = value;
                    });
                }
            });
        }

        protected override ContentControl Map(View view, IShibaContext context)
        {
            var result = base.Map(view, context);
            result.SetValue(ContentControl.ContentTemplateSelectorProperty, result.Tag);
            return result;
        }
    }
}
