using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WeiPo.Services.Models;
using WeiPo.ViewModels.User.Tab;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Activities.User.Tab
{
    public class WeiboTabDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StatusTemplate { get; set; }
        public DataTemplate InterestTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            switch (item)
            {
                case StatusModel status:
                    return StatusTemplate;
                case InterestPeopleViewModel interest:
                    return InterestTemplate;
            }

            return new DataTemplate();
        }
    }

    public sealed partial class WeiboTab
    {
        public WeiboTab()
        {
            InitializeComponent();
        }

        protected override AbsTabViewModel CreateViewModel(ProfileData viewModelProfile, Services.Models.Tab tabData)
        {
            return new WeiboTabViewModel(viewModelProfile, tabData);
        }
    }
}