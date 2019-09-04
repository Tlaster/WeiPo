using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class NotificationViewModel : ViewModelBase
    {
        private readonly int _duration = 60 * 1000;
        private bool _isLoginCompleted;
        private Task _task;

        private NotificationViewModel()
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
            Singleton<MessagingCenter>.Instance.Subscribe("notification_clear_fans", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Unread.Follower = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });
            Singleton<MessagingCenter>.Instance.Subscribe("notification_clear_mention_at", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Unread.MentionStatus = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });

            Singleton<MessagingCenter>.Instance.Subscribe("notification_clear_mention_comment", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Unread.MentionCmt = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });

            Singleton<MessagingCenter>.Instance.Subscribe("notification_clear_comment", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Unread.Cmt = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });
        }

        public static NotificationViewModel Instance { get; } = new NotificationViewModel();
        public UnreadModel Unread { get; set; }

        private async Task FetchUnread()
        {
            Debug.WriteLine("fetching notification...");
            var result = await Singleton<Api>.Instance.Unread();
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => Unread = result);
            Debug.WriteLine("fetching complete!");
        }
    }
}