using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Nito.Mvvm;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        private int _duration = 60 * 1000;
        private bool _isLoginCompleted = false;
        private Task _task;
        public UnreadModel Unread { get; set; }

        public NotificationViewModel()
        {
            Singleton<MessagingCenter>.Instance.Subscribe("login_completed", delegate
            {
                _isLoginCompleted = true;
                
                _task = Task.Run(async () =>
                {
                    while (true)
                    {
                        if (_isLoginCompleted)
                        {
                            try
                            {
                                await FetchUnread();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                Debug.WriteLine(e.Message);
                                Debug.WriteLine(e.StackTrace);
                            }
                        }
                        await Task.Delay(_duration);
                    }
                });
            });
        }

        private async Task FetchUnread()
        {
            Debug.WriteLine("fetching notification...");
            var result = await Singleton<Api>.Instance.Unread();
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => Unread = result.Data);
            Debug.WriteLine("fetching complete!");
        }
    }
    public class DockViewModel : ViewModelBase
    {
        public DockViewModel()
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