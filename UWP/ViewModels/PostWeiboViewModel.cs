using Newtonsoft.Json.Linq;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace WeiPo.ViewModels
{
    public enum PostType
    {
        Create,
        Repost,
        Comment
    }

    public class PostWeiboViewModel : ViewModelBase
    {
        private readonly Encoding _gb2312 = Encoding.GetEncoding("gb2312");

        public PostWeiboViewModel()
        {
            Files.CollectionChanged += FilesOnCollectionChanged;
            Singleton<BroadcastCenter>.Instance.Subscribe("status_share", (sender, args) =>
            {
                if (args is StatusModel status)
                {
                    ToRepostState(status);
                }
            });
            Singleton<BroadcastCenter>.Instance.Subscribe("status_comment", (sender, args) =>
            {
                if (args is ICanReply reply)
                {
                    ToCommentState(reply);
                }
            });
        }

        public bool IsSending { get; private set; }

        [DependsOn(nameof(IsSending))] public bool IsNotSending => !IsSending;

        public PostType PostType { get; set; }

        public ICanReply ReplyModel { get; private set; }
        public int MaxImageFileCount { get; set; } = 9;
        public string Content { get; set; } = string.Empty;
        public int MaxLength { get; set; } = 1000;
        public ObservableCollection<StorageFile> Files { get; } = new ObservableCollection<StorageFile>();

        public async Task Commit()
        {
            var textLength = _gb2312.GetByteCount(Content) / 2d;
            if (textLength > MaxLength || string.IsNullOrEmpty(Content))
            {
                //TODO: notify text out of range
                return;
            }

            IsSending = true;
            var content = Content.Replace("\r", "\n");//multi line
            var files = Files.ToList();
            var picids = await Task.WhenAll(files.Select(async it => await Singleton<Api>.Instance.UploadPic(it)));
            JObject result;
            switch (PostType)
            {
                case PostType.Create:
                    {
                        result = await Singleton<Api>.Instance.Update(content, picids.Select(it => it.PicId).ToArray());
                    }
                    break;
                case PostType.Repost:
                    {
                        result = await Singleton<Api>.Instance.Repost(content, ReplyModel, picids.FirstOrDefault()?.PicId);
                    }
                    break;
                case PostType.Comment:
                    {
                        if (ReplyModel is CommentModel comment)
                        {
                            result =
                                await Singleton<Api>.Instance.Reply(content, comment, picids.FirstOrDefault()?.PicId);
                        }
                        else
                        {
                            result =
                                await Singleton<Api>.Instance.Comment(content, ReplyModel, picids.FirstOrDefault()?.PicId);
                        }
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            IsSending = false;
            Singleton<BroadcastCenter>.Instance.Send(this, "post_weibo_complete", result);
        }

        public async Task PickImages()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");
            var files = await picker.PickMultipleFilesAsync();
            AddImage(files.ToArray());
        }

        public void AddImage(params StorageFile[] files)
        {
            var images = Files.Concat(files).Take(MaxImageFileCount).ToList();
            Files.Clear();
            foreach (var item in images)
            {
                Files.Add(item);
            }
        }

        public void ToCommentState(ICanReply model)
        {
            IsSending = false;
            PostType = PostType.Comment;
            ReplyModel = model;
            MaxImageFileCount = 1;
            Content = string.Empty;
            MaxLength = 140;
            Files.Clear();
        }

        public void ToCreateState()
        {
            IsSending = false;
            PostType = PostType.Create;
            ReplyModel = null;
            MaxImageFileCount = 9;
            Content = string.Empty;
            MaxLength = 1000;
            Files.Clear();
        }

        public void ToRepostState(StatusModel model)
        {
            IsSending = false;
            PostType = PostType.Repost;
            ReplyModel = model.RetweetedStatus ?? model;
            MaxImageFileCount = 1;
            Content = model.RetweetedStatus == null ? string.Empty : $"//@{model.User.ScreenName}:{model.RawText}";
            MaxLength = 140;
            Files.Clear();
        }

        private void FilesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Singleton<BroadcastCenter>.Instance.Send(this, "dock_image_count_changed", Files.Count);
        }
    }
}