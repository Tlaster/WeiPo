using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;

namespace WeiPo.Common
{
    public static class ToastNotificationSender
    {
        public static void SendText(string text)
        {
            var content = new ToastContent()
            {
                Scenario = ToastScenario.Default,
                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = text
                            }
                        }
                    }
                }
            };
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
        }
    }
}
