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
        private Task _task;

        private NotificationViewModel()
        {
        }

        public static NotificationViewModel Instance { get; } = new NotificationViewModel();

        public void Init()
        {
            _task = Task.Run(async () =>
            {
                while (true)
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
                    await Task.Delay(_duration);
                }
            });
        }
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
            if (newValue.Follower != oldValue?.Follower)
            {
                if (newValue.Follower != 0)
                {
                    Singleton<BroadcastCenter>.Instance.Send(this, "notification_new_fans", newValue.Follower);
                }
                Singleton<BroadcastCenter>.Instance.Send(this, "notification_changed_fans", newValue.Follower);
            }

            if (newValue.MentionStatus != oldValue?.MentionStatus)
            {
                if (newValue.MentionStatus != 0)
                {
                    Singleton<BroadcastCenter>.Instance.Send(this, "notification_new_mention_at", newValue.MentionStatus);
                }
                Singleton<BroadcastCenter>.Instance.Send(this, "notification_changed_mention_at", newValue.MentionStatus);
            }

            if (newValue.MentionCmt != oldValue?.MentionCmt)
            {
                if (newValue.MentionCmt != 0)
                {
                    Singleton<BroadcastCenter>.Instance.Send(this, "notification_new_mention_comment", newValue.MentionCmt);
                }
                Singleton<BroadcastCenter>.Instance.Send(this, "notification_changed_mention_comment", newValue.MentionCmt);
            }

            if (newValue.Cmt != oldValue?.Cmt)
            {
                if (newValue.Cmt != 0)
                {
                    Singleton<BroadcastCenter>.Instance.Send(this, "notification_new_comment", newValue.Cmt);
                }
                Singleton<BroadcastCenter>.Instance.Send(this, "notification_changed_comment", newValue.Cmt);
            }

            if (newValue.Dm != oldValue?.Dm)
            {
                if (newValue.Dm != 0)
                {
                    Singleton<BroadcastCenter>.Instance.Send(this, "notification_new_dm", newValue.Dm);
                }
                Singleton<BroadcastCenter>.Instance.Send(this, "notification_changed_dm", newValue.Dm);
            }
        }
    }
}