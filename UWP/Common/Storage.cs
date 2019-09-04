using Windows.Storage;

namespace WeiPo.Common
{
    internal class Storage
    {
        public T Load<T>(string name, T defaultValue = default)
        {
            var result = ApplicationData.Current.LocalSettings.Values[name];
            if (result is T target)
            {
                return target;
            }

            return defaultValue;
        }

        public void Save<T>(string name, T value)
        {
            ApplicationData.Current.LocalSettings.Values[name] = value;
        }
    }
}