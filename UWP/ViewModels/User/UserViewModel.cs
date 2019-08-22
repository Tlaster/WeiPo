using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User
{
    public class UserViewModel : ViewModelBase
    {
        public bool IsLoading { get; private set; }
        public ProfileData Profile { get; private set; }

        public UserViewModel(UserModel user)
        {
            Init(user);
        }

        public UserViewModel(string name)
        {
            Init(name);
        }

        private async void Init(string name)
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;
            var response = await Singleton<Api>.Instance.Profile(name);
            if (response.Ok == 1)
            {
                Profile = response.Data;
            }
            else
            {

            }

            IsLoading = false;
        }

        private async void Init(UserModel user)
        {
            if (IsLoading)
            {
                return;
            }
            
            IsLoading = true;
            var response = await Singleton<Api>.Instance.Profile(user.Id);
            if (response.Ok == 1)
            {
                Profile = response.Data;
            }
            else
            {

            }

            IsLoading = false;
        }
    }
}
