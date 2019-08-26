using System.Threading.Tasks;
using Nito.Mvvm;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class DockViewModel : ViewModelBase
    {
        public DockViewModel()
        {
            Singleton<MessagingCenter>.Instance.Subscribe("login_completed",
                (sender, args) => MyProfile = NotifyTask.Create(LoadMe));
        }

        public PostWeiboViewModel PostWeiboViewModel { get; } = new PostWeiboViewModel();
        public NotifyTask<ProfileData> MyProfile { get; private set; }

        public void ToMyProfile()
        {
            if (MyProfile.IsCompleted)
            {
                Singleton<MessagingCenter>.Instance.Send(this, "user_clicked", MyProfile.Result.UserInfo.Id);
            }
        }

        private async Task<ProfileData> LoadMe()
        {
            var result = await Singleton<Api>.Instance.Me();
            if (result.Ok == 1) return result.Data;
            return null;
        }
    }
}