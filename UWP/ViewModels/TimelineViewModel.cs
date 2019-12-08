using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.Extensions;
using Nito.Mvvm;
using WeiPo.Common;
using WeiPo.Common.Collection;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class TimelineDataSource : IIncrementalSource<StatusModel>
    {
        private long _maxId;

        public async Task<IEnumerable<StatusModel>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (pageIndex == 0) _maxId = 0;

            var result = await Singleton<Api>.Instance.Timeline(_maxId, cancellationToken);
            if (result == null) return new List<StatusModel>();

            var list = result.Statuses;
            _maxId = result.NextCursor;
            return list;
        }
    }

    public abstract class NavigationViewModelWithNotification : ViewModelBase
    {
        protected NavigationViewModelWithNotification()
        {
            if (!string.IsNullOrEmpty(NotificationName))
            {
                Singleton<BroadcastCenter>.Instance.Subscribe(NotificationName, (sender, args) =>
                {
                    if (args is long longArgs) NotificationCount = longArgs;
                });
            }

            if (Source is IWithStatus withStatus)
            {
                withStatus.OnStartLoading += () => NotificationCount = 0;
            }
        }

        public long NotificationCount { get; set; }
        public abstract string Name { get; set; }
        public string Title => Name.GetLocalized();
        public abstract Symbol Icon { get; set; }
        public abstract string NotificationName { get; }
        public abstract object Source { get; }
    }
    
    public class TimelineNavigationViewModel : NavigationViewModelWithNotification
    {
        public override string Name { get; set; } = "Timeline";
        public override Symbol Icon { get; set; } = Symbol.Home;
        public override string NotificationName { get; } = "";

        public override object Source { get; } =
            new LoadingCollection<IIncrementalSource<StatusModel>, StatusModel>(new TimelineDataSource());
    }

    public class MentionNavigationViewModel : NavigationViewModelWithNotification
    {
        public override string Name { get; set; } = "Mention";
        public override Symbol Icon { get; set; } = Symbol.Account;
        public override string NotificationName { get; } = "notification_new_mention_at";

        public override object Source { get; } =
            new LoadingCollection<IIncrementalSource<StatusModel>, StatusModel>(
                new MessagingCenterDockItemDataSource<StatusModel>("notification_clear_mention_at",
                    async page => await Singleton<Api>.Instance.GetMentionsAt(page)));
    }

    public class MentionCommentNavigationViewModel : NavigationViewModelWithNotification
    {
        public override string Name { get; set; } = "MentionComment";
        public override Symbol Icon { get; set; } = Symbol.Account;
        public override string NotificationName { get; } = "notification_new_mention_comment";
        public override object Source { get; } = 
            new LoadingCollection<IIncrementalSource<CommentModel>, CommentModel>(
                    new MessagingCenterDockItemDataSource<CommentModel>("notification_clear_mention_comment",
                        async page => await Singleton<Api>.Instance.GetMentionsCmt(page)));
    }

    public class CommentNavigationViewModel : NavigationViewModelWithNotification
    {
        public override string Name { get; set; } = "Comment";
        public override Symbol Icon { get; set; } = Symbol.Comment;
        public override string NotificationName { get; } = "notification_new_comment";

        public override object Source { get; } =
            new LoadingCollection<IIncrementalSource<CommentModel>, CommentModel>(
                new MessagingCenterDockItemDataSource<CommentModel>("notification_clear_comment",
                    async page => await Singleton<Api>.Instance.GetComment(page)));
    }

    public class LikeNavigationViewModel : NavigationViewModelWithNotification
    {
        public override string Name { get; set; } = "Like";
        public override Symbol Icon { get; set; } = Symbol.Like;
        public override string NotificationName { get; } = string.Empty;
        public override object Source { get; } =
            new LoadingCollection<IIncrementalSource<AttitudeModel>, AttitudeModel>(
                    new MessagingCenterDockItemDataSource<AttitudeModel>("",
                        async page => await Singleton<Api>.Instance.GetAttitude(page)));
    }

    public class DirectMessageNavigationViewModel : NavigationViewModelWithNotification
    {
        public override string Name { get; set; } = "DirectMessage";
        public override Symbol Icon { get; set; } = Symbol.Message;
        public override string NotificationName { get; } = "notification_new_dm";
        public override object Source { get; } = 
            new LoadingCollection<IIncrementalSource<MessageListModel>, MessageListModel>(
                    new MessagingCenterDockItemDataSource<MessageListModel>("notification_clear_dm",
                        async page => await Singleton<Api>.Instance.GetMessageList(page)));
    }

    public class TimelineViewModel : ViewModelBase
    {
        public TimelineViewModel()
        {
            MyProfile = NotifyTask.Create(LoadMe);
        }

        public List<NavigationViewModelWithNotification> Source { get; } = new List<NavigationViewModelWithNotification>
        {
            new TimelineNavigationViewModel(),
            new MentionNavigationViewModel(),
            new MentionCommentNavigationViewModel(),
            new CommentNavigationViewModel(),
            new LikeNavigationViewModel(),
            new DirectMessageNavigationViewModel(),
        };

        public NotifyTask<ProfileData> MyProfile { get; }

        private async Task<ProfileData> LoadMe()
        {
            return await Singleton<Api>.Instance.Me();
        }

        public void ToMyProfile()
        {
            if (MyProfile.IsCompleted && MyProfile.Result != null)
                Singleton<BroadcastCenter>.Instance.Send(this, "user_clicked", MyProfile.Result.UserInfo.Id);
        }
    }
}