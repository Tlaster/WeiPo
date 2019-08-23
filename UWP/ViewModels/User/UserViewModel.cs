using System.Threading.Tasks;
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
            Init(user.Id);
        }

        public UserViewModel(string name)
        {
            Init(name);
        }

        public UserViewModel(long id)
        {
            Init(id);
        }

        private async void Init(long id)
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;
            await InitProfile(id);
            IsLoading = false;
        }

        private async void Init(string name)
        {
            if (IsLoading)
            {
                return;
            }

            IsLoading = true;
            var id = await Singleton<Api>.Instance.UserId(name);
            await InitProfile(id);
            IsLoading = false;
        }

        private async Task InitProfile(long id)
        {
            var response = await Singleton<Api>.Instance.Profile(id);
            if (response.Ok == 1)
            {
                Profile = response.Data;
            }
            else
            {

            }
        }

    }
}
