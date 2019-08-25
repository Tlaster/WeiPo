using System;
using System.Threading.Tasks;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User
{
    public class UserViewModel : ViewModelBase
    {
        public UserViewModel(UserModel user, Action updatePivot)
        {
            UpdatePivot = updatePivot;
            Init(user.Id);
        }

        public UserViewModel(string name, Action updatePivot)
        {
            UpdatePivot = updatePivot;
            Init(name);
        }

        public UserViewModel(long id, Action updatePivot)
        {
            UpdatePivot = updatePivot;
            Init(id);
        }

        public bool IsLoading { get; private set; }
        public ProfileData Profile { get; private set; }

        public Action UpdatePivot { get; }

        private async void Init(long id)
        {
            if (IsLoading) return;

            IsLoading = true;
            await InitProfile(id);
            IsLoading = false;
        }

        private async void Init(string name)
        {
            if (IsLoading) return;

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
                UpdatePivot.Invoke();
            }
        }
    }
}