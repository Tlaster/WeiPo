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
        public static NotificationViewModel Instance { get; } = new NotificationViewModel();
        private int _duration = 60 * 1000;
        private bool _isLoginCompleted = false;
        private Task _task;
        public UnreadModel Unread { get; set; }

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
        }

        private async Task FetchUnread()
        {
            Debug.WriteLine("fetching notification...");
            var result = await Singleton<Api>.Instance.Unread();
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => Unread = result);
            Debug.WriteLine("fetching complete!");
        }
    }
}