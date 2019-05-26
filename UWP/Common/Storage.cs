using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace WeiPo.Common
{
    class Storage
    {
        public void Save<T>(string name, T value)
        {
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }

        public T Load<T>(string name, T defaultValue = default)
        {
            var result = ApplicationData.Current.LocalSettings.Values[name];
            if (result is T target)
            {
                return target;
            }

            return defaultValue;
        }
    }
}
