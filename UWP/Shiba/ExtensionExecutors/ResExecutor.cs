using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Shiba.Controls;
using Shiba.ExtensionExecutors;

namespace WeiPo.Shiba.ExtensionExecutors
{
    public class ResExecutor : IMutableExtensionExecutor
    {
        public object ProvideValue(IShibaContext context, ShibaExtension value)
        {
            return Application.Current.Resources.ContainsKey(value.Value) ? Application.Current.Resources[value.Value] : null;
        }

        public string Name { get; } = "res";
    }
}
