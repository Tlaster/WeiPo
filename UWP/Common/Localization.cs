using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Media.Streaming.Adaptive;
using Windows.UI.Xaml.Markup;
using Microsoft.Toolkit.Uwp.Extensions;

namespace WeiPo.Common
{
    class i18n : MarkupExtension
    {
        public string Key { get; set; } 
        protected override object ProvideValue()
        {
            return Localization.GetString(Key);
        }
    }

    static class Localization 
    {
        public static string GetString(string key)
        {
            return key.GetViewLocalized();
        }
        
        public static string Format(string key, object arg)
        {
            return string.Format(key.GetViewLocalized(), arg);
        }

        public static string Format(string key, object arg0, object arg1)
        {
            return string.Format(key.GetViewLocalized(), arg0, arg1);
        }
    }
}
