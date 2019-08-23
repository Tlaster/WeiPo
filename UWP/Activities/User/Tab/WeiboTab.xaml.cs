using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WeiPo.Services.Models;
using WeiPo.ViewModels.User.Tab;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Activities.User.Tab
{
    public class WeiboTabDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StatusTemplate { get; set; }
        public DataTemplate InterestTemplate { get; set; }
        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            switch (item)
            {
                case StatusModel status:
                    return StatusTemplate;
                case InterestPeopleModel interest:
                    return InterestTemplate;
            }
            return new DataTemplate();
        }
    }

    public sealed partial class WeiboTab
    {
        public WeiboTab()
        {
            this.InitializeComponent();
        }

        protected override AbsTabViewModel CreateViewModel(ProfileData viewModelProfile, Services.Models.Tab tabData)
        {
            return new WeiboTabViewModel(viewModelProfile, tabData);
        }

        private async void ScrollViewer_OnViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                var scroller = (ScrollViewer)sender;
                var distanceToEnd = scroller.ExtentHeight - (scroller.VerticalOffset + scroller.ViewportHeight);
                // trigger if within 2 viewports of the end
                if (ViewModel is WeiboTabViewModel viewModel && distanceToEnd <= 2.0 * scroller.ViewportHeight
                    && viewModel.DataSource.HasMoreItems && !viewModel.DataSource.IsLoading)
                {
                    await viewModel.DataSource.LoadMoreItemsAsync(20);
                }
            }
        }
    }
}
