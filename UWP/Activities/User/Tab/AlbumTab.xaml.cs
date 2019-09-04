using WeiPo.Services.Models;
using WeiPo.ViewModels.User.Tab;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Activities.User.Tab
{
    public sealed partial class AlbumTab
    {
        public AlbumTab()
        {
            InitializeComponent();
        }

        protected override AbsTabViewModel CreateViewModel(ProfileData viewModelProfile, Services.Models.Tab tabData)
        {
            return new AlbumTabViewModel(viewModelProfile, tabData);
        }
    }
}