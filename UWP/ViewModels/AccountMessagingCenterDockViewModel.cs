using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp.Extensions;
using WeiPo.Common;
using WeiPo.Common.Collection;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class MessagingCenterDockItemDataSource<T> : IIncrementalSource<T>
    {
        private readonly Func<int, Task<IEnumerable<T>>> _func;
        private readonly string _postMessageId;

        public MessagingCenterDockItemDataSource(string postMessageId, Func<int, Task<IEnumerable<T>>> func)
        {
            _postMessageId = postMessageId;
            _func = func;
        }

        public async Task<IEnumerable<T>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (pageIndex == 0 && !string.IsNullOrEmpty(_postMessageId))
                //clear notification
            {
                Singleton<MessagingCenter>.Instance.Send(this, _postMessageId);
            }

            var result = await _func.Invoke(pageIndex + 1);
            return result;
        }
    }

    public class MessagingCenterDockItemViewModel : ViewModelBase
    {
        public MessagingCenterDockItemViewModel(string id, Symbol symbol, object source)
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

    public class AccountMessagingCenterDockViewModel : ViewModelBase
    {
        public List<MessagingCenterDockItemViewModel> Source { get; } = new List<MessagingCenterDockItemViewModel>
        {
            new MessagingCenterDockItemViewModel("Mention", Symbol.Account,
                new LoadingCollection<MessagingCenterDockItemDataSource<StatusModel>, StatusModel>(
                    new MessagingCenterDockItemDataSource<StatusModel>("notification_clear_mention_at",
                        async page => await Singleton<Api>.Instance.GetMentionsAt(page)))),
            new MessagingCenterDockItemViewModel("MentionComment", Symbol.Account,
                new LoadingCollection<MessagingCenterDockItemDataSource<CommentModel>, CommentModel>(
                    new MessagingCenterDockItemDataSource<CommentModel>("notification_clear_mention_comment",
                        async page => await Singleton<Api>.Instance.GetMentionsCmt(page)))),
            new MessagingCenterDockItemViewModel("Comment", Symbol.Comment,
                new LoadingCollection<MessagingCenterDockItemDataSource<CommentModel>, CommentModel>(
                    new MessagingCenterDockItemDataSource<CommentModel>("notification_clear_comment",
                        async page => await Singleton<Api>.Instance.GetComment(page)))),
            new MessagingCenterDockItemViewModel("Like", Symbol.Like,
                new LoadingCollection<MessagingCenterDockItemDataSource<AttitudeModel>, AttitudeModel>(
                    new MessagingCenterDockItemDataSource<AttitudeModel>("",
                        async page => await Singleton<Api>.Instance.GetAttitude(page)))),
            new MessagingCenterDockItemViewModel("DirectMessage", Symbol.Message,
            new LoadingCollection<MessagingCenterDockItemDataSource<MessageListModel>, MessageListModel>(
                new MessagingCenterDockItemDataSource<MessageListModel>("notification_clear_dm",
                    async page => await Singleton<Api>.Instance.GetMessageList(page))))
        };
    }
}