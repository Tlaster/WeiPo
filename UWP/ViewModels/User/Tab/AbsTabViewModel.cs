using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User.Tab
{
    public abstract class AbsTabViewModel : ViewModelBase
    {
        protected AbsTabViewModel(ProfileData profile, Services.Models.Tab tabData)
        {
            ProfileData = profile;
            Tab = tabData;
        }

        public ProfileData ProfileData { get; }
        public Services.Models.Tab Tab { get; }
    }
}