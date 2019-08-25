using Windows.UI.Core;
using Windows.UI.Xaml;
using WeiPo.Services.Models;
using WeiPo.ViewModels;
using WeiPo.ViewModels.User;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeiPo.Activities.User
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UserActivity
    {
        public UserViewModel ViewModel { get; private set; }
        public UserActivity()
        {
            this.InitializeComponent();
        }

        protected override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            switch (parameter)
            {
                case UserModel user:
                    ViewModel = new UserViewModel(user, UpdatePivot);
                    break;
                case string name:
                    ViewModel = new UserViewModel(name, UpdatePivot);
                    break;
                case long id:
                    ViewModel = new UserViewModel(id, UpdatePivot);
                    break;
            }
        }

        private void UpdatePivot()
        {
            //Don't know why data binding not work for Pivot.SelectedIndex property, here is a workaround
            Dispatcher.RunAsync(CoreDispatcherPriority.Low,
                () => this.ContentPivot.SelectedIndex = ViewModel.Profile.TabsInfo.SelectedTab);
        }
    }
}
