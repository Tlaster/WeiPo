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
            if (pageIndex == 0)
            {
                _maxId = 0;
            }

            var result = await Singleton<Api>.Instance.Timeline(_maxId, cancellationToken);
            if (result == null)
            {
                return new List<StatusModel>();
            }

            var list = result.Statuses;
            _maxId = result.NextCursor;
            return list;
        }
    }

    public class TimelineNavigationSubViewModel : ViewModelBase
    {
        public TimelineNavigationSubViewModel(string id, Symbol symbol, object source)
        {
            Id = id;
            Symbol = symbol;
            Source = source;
        }

        public string Id { get; }
        public string Title => Id.GetLocalized();
        public Symbol Symbol { get; }
        public object Source { get; }
    }

    public class TimelineViewModel : ViewModelBase
    {
        public TimelineViewModel()
        {
            MyProfile = NotifyTask.Create(LoadMe);
        }

        public List<TimelineNavigationSubViewModel> Sources { get; } = new List<TimelineNavigationSubViewModel>
        {
            new TimelineNavigationSubViewModel("Timeline", Symbol.Home,
                new LoadingCollection<TimelineDataSource, StatusModel>()),

            new TimelineNavigationSubViewModel("Mention", Symbol.Account,
                new LoadingCollection<MessagingCenterDockItemDataSource<StatusModel>, StatusModel>(
                    new MessagingCenterDockItemDataSource<StatusModel>("notification_clear_mention_at",
                        async page => await Singleton<Api>.Instance.GetMentionsAt(page)))),

            new TimelineNavigationSubViewModel("MentionComment", Symbol.Account,
                new LoadingCollection<MessagingCenterDockItemDataSource<CommentModel>, CommentModel>(
                    new MessagingCenterDockItemDataSource<CommentModel>("notification_clear_mention_comment",
                        async page => await Singleton<Api>.Instance.GetMentionsCmt(page)))),

            new TimelineNavigationSubViewModel("Comment", Symbol.Comment,
                new LoadingCollection<MessagingCenterDockItemDataSource<CommentModel>, CommentModel>(
                    new MessagingCenterDockItemDataSource<CommentModel>("notification_clear_comment",
                        async page => await Singleton<Api>.Instance.GetComment(page)))),

            new TimelineNavigationSubViewModel("Like", Symbol.Like,
                new LoadingCollection<MessagingCenterDockItemDataSource<AttitudeModel>, AttitudeModel>(
                    new MessagingCenterDockItemDataSource<AttitudeModel>("",
                        async page => await Singleton<Api>.Instance.GetAttitude(page)))),

            new TimelineNavigationSubViewModel("DirectMessage", Symbol.Message,
                new LoadingCollection<MessagingCenterDockItemDataSource<MessageListModel>, MessageListModel>(
                    new MessagingCenterDockItemDataSource<MessageListModel>("notification_clear_dm",
                        async page => await Singleton<Api>.Instance.GetMessageList(page))))
        };

        public NotifyTask<ProfileData> MyProfile { get; }

        private async Task<ProfileData> LoadMe()
        {
            return await Singleton<Api>.Instance.Me();
        }

        public void ToMyProfile()
        {
            if (MyProfile.IsCompleted && MyProfile.Result != null)
            {
                Singleton<BroadcastCenter>.Instance.Send(this, "user_clicked", MyProfile.Result.UserInfo.Id);
            }
        }
    }
}