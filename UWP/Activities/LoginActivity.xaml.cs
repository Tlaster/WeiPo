// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http.Filters;
using WeiPo.Common;

namespace WeiPo.Activities
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginActivity
    {
        public LoginActivity()
        {
            InitializeComponent();
        }

        protected override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            var requri = new Uri("https://m.weibo.cn/login?backURL=https%253A%252F%252Fm.weibo.cn%252F");
            LoginWebView.NavigationCompleted += LoginWebViewOnNavigationCompleted;
            LoginWebView.Navigate(requri);
        }

        private void LoginWebViewOnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var httpBaseProtocolFilter = new HttpBaseProtocolFilter();
            var cookieManager = httpBaseProtocolFilter.CookieManager;
            var cookieCollection = cookieManager.GetCookies(new Uri("https://m.weibo.cn/"));
            if (cookieCollection != null && cookieCollection.Any(it => it.Name == "MLOGIN" && it.Value == "1"))
            {
                Singleton<Storage>.Instance.Save("usercookie",
                    cookieCollection.ToDictionary(it => it.Name, it => it.Value).ToJson());
                Singleton<MessagingCenter>.Instance.Send(this, "login_completed");
                Finish();
            }
        }
    }
}