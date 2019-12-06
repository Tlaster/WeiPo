using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Extensions;
using PropertyChanged;
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

        public ProfileData Me { get; private set; }

        [DependsOn(nameof(Profile))]
        public Services.Models.Tab[] Tabs
        {
            get
            {
                return Profile?.TabsInfo?.Tabs?.Concat(new[]
                {
                    new Services.Models.Tab
                    {
                        Title = "Follow".GetLocalized(),
                        TabType = "follow",
                        Containerid = Profile.UserInfo.Id.ToString()
                    },
                    new Services.Models.Tab
                    {
                        Title = "Fans".GetLocalized(),
                        TabType = "fans",
                        Containerid = Profile.UserInfo.Id.ToString()
                    }
                }).ToArray();
            }
        }

        [DependsOn(nameof(Profile))]
        public bool IsNotMe => Profile?.UserInfo?.Id != Me?.UserInfo?.Id;

        public Action UpdatePivot { get; }

        public async Task UpdateFollowState()
        {
            if (IsLoading)
            {
                return;
            }
            IsLoading = true;
            UserModel model;
            if (Profile.UserInfo.Following)
            {
                model = await Singleton<Api>.Instance.Unfollow(Profile.UserInfo.Id);
            }
            else
            {
                model = await Singleton<Api>.Instance.Follow(Profile.UserInfo.Id);
            }
            Profile.UserInfo.Following = !Profile.UserInfo.Following;
            OnPropertyChanged(nameof(Profile));
            IsLoading = false;
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
            Me = await Singleton<Api>.Instance.Me();
            Profile = await Singleton<Api>.Instance.Profile(id);
            UpdatePivot.Invoke();
        }

    }
}