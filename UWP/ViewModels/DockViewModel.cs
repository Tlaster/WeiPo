using System.Threading.Tasks;
using Nito.Mvvm;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;
namespace WeiPo.ViewModels
{
    public class DockViewModel : ViewModelBase
    {
        public static DockViewModel Instance { get; } = new DockViewModel();
        private DockViewModel()
        {
            Singleton<MessagingCenter>.Instance.Subscribe("login_completed",
                (sender, args) =>
                {
                    MyProfile = NotifyTask.Create(LoadMe);
                });
        }

        public PostWeiboViewModel PostWeiboViewModel { get; } = new PostWeiboViewModel();
        public NotificationViewModel NotificationViewModel { get; } = new NotificationViewModel();
        public NotifyTask<ProfileData> MyProfile { get; private set; }

        public void ToMyProfile()
        {
            if (MyProfile.IsCompleted && MyProfile.Result != null)
            {
                Singleton<MessagingCenter>.Instance.Send(this, "user_clicked", MyProfile.Result.UserInfo.Id);
            }
        }

        private async Task<ProfileData> LoadMe()
        {
            return await Singleton<Api>.Instance.Me();
        }
    }
}