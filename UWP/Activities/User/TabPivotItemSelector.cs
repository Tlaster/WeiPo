using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WeiPo.Activities.User
{
    internal class TabPivotDictionary : Dictionary<string, DataTemplate>
    {
    }

    internal class TabPivotItemSelector : DataTemplateSelector
    {
        public TabPivotDictionary Mapping { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is Services.Models.Tab tab && Mapping != null &&
                Mapping.TryGetValue(tab.TabType, out var template))
            {
                return template;
            }

            return new DataTemplate();
        }
    }
}