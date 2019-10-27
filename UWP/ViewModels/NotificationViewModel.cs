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
            Singleton<BroadcastCenter>.Instance.Subscribe("login_completed", delegate
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
            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_fans", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Unread.Follower = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });
            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_mention_at", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Unread.MentionStatus = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });

            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_mention_comment", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Unread.MentionCmt = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });
            
            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_comment", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Unread.Cmt = 0;
                    OnPropertyChanged(nameof(Unread));
                });
            });

            Singleton<BroadcastCenter>.Instance.Subscribe("notification_clear_dm", delegate
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Unread.Dm = 0;
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
            
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                SendToastNotification(result, Unread);
                Unread = result;
            });
            Debug.WriteLine("fetching complete!");
        }

        private void SendToastNotification(UnreadModel newValue, UnreadModel oldValue)
        {
            if (newValue.Follower != 0 && newValue.Follower != oldValue?.Follower)
            {
                ToastNotificationSender.SendText(Localization.Format("FollowerCount", newValue.Follower));
            }

            if (newValue.MentionStatus != 0 && newValue.MentionStatus != oldValue?.MentionStatus)
            {
                ToastNotificationSender.SendText(Localization.Format("MentionStatusCount", newValue.MentionStatus));
            }

            if (newValue.MentionCmt != 0 && newValue.MentionCmt != oldValue?.MentionCmt)
            {
                ToastNotificationSender.SendText(Localization.Format("MentionCmtCount", newValue.MentionCmt));
            }

            if (newValue.Cmt != 0 && newValue.Cmt != oldValue?.Cmt)
            {
                ToastNotificationSender.SendText(Localization.Format("CmtCount", newValue.Cmt));
            }

            if (newValue.Dm != 0 && newValue.Dm != oldValue?.Dm)
            {
                ToastNotificationSender.SendText(Localization.Format("DmCount", newValue.Dm));
            }
        }
    }
}