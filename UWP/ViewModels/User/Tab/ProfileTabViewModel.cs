using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User.Tab
{
    public class ProfileTabViewModel : AbsTabViewModel
    {
        public ProfileTabViewModel(ProfileData profile, Services.Models.Tab tabData) : base(profile, tabData)
        {
        }
    }
}