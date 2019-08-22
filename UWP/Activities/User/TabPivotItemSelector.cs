using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace WeiPo.Activities.User
{
    class TabPivotDictionary : Dictionary<string, DataTemplate>
    {
        
    }
    class TabPivotItemSelector : DataTemplateSelector
    {
        public TabPivotDictionary Mapping { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is Services.Models.Tab tab && Mapping != null && Mapping.TryGetValue(tab.TabType, out var template))
            {
                return template;
            }
            return new DataTemplate();
        }
    }
}
