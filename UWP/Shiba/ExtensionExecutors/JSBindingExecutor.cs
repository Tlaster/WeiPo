using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Shiba;
using Shiba.Controls;
using Shiba.ExtensionExecutors;
using Shiba.Internal;

namespace WeiPo.Shiba.ExtensionExecutors
{
    internal class JSBindingConverter : IValueConverter
    {
        private JSBindingConverter()
        {
            
        }
        public static JSBindingConverter Instance { get; } = new JSBindingConverter();
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var targetPath = parameter + "";

            if (string.IsNullOrEmpty(targetPath)) return value;

            var result = targetPath.Split('.').Aggregate(value, (current, path) => ShibaApp.Instance.Configuration.ScriptRuntime.GetProperty(current, path));
            if (ShibaApp.Instance.Configuration.ScriptRuntime.IsArray(result))
            {
                return ShibaApp.Instance.Configuration.ScriptRuntime.ToArray(result);
            }
            else
            {
                return result;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class JSBindingExecutor : BindingExecutor
    {
        public override string Name { get; } = "jsbind";

        public override Binding ProvideNativeBinding(IShibaContext context, ShibaExtension value)
        {
            var dataContextPath =
                "DataContext";
            var targetPath = value.Value;
            var path = dataContextPath;

            return new Binding
            {
                Converter = JSBindingConverter.Instance,
                ConverterParameter = targetPath,
                Source = context.ShibaHost,
                Path = new PropertyPath(path),
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
        }
    }
}
