using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Newtonsoft.Json.Linq;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

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
        private bool _isSending;
        private StatusModel _statusModel;

        public bool IsSending
        {
            get => _isSending;
            private set
            {
                _isSending = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotSending));
            }
        }

        public bool IsNotSending => !IsSending;

        public PostType PostType { get; set; }

        public StatusModel StatusModel
        {
            get => _statusModel;
            private set
            {
                _statusModel = value;
                OnPropertyChanged();
            }
        }

        public int MaxImageFileCount { get; set; } = 9;
        public string Content { get; set; } = string.Empty;
        public int MaxLength { get; set; } = 1000;
        public ObservableCollection<StorageFile> Files { get; } = new ObservableCollection<StorageFile>();

        public PostWeiboViewModel()
        {
            Singleton<MessagingCenter>.Instance.Subscribe("clear_dock_compose", (sender, args) => ToCreateState());
            Singleton<MessagingCenter>.Instance.Subscribe("status_share", (sender, args) =>
            {
                if (args is StatusModel status)
                {
                    ToRepostState(status);
                }
            });
            Singleton<MessagingCenter>.Instance.Subscribe("status_comment", (sender, args) =>
            {
                if (args is StatusModel status)
                {
                    ToCommentState(status);
                }
            });
        }

        public async Task PickImages()
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");
            var files = await picker.PickMultipleFilesAsync();
            Files.Clear();
            foreach (var item in Files.Concat(files).Take(MaxImageFileCount)) Files.Add(item);
        }
        
        public void ToCreateState()
        {
            IsSending = false;
            PostType = PostType.Create;
            StatusModel = null;
            MaxImageFileCount = 9;
            Content = string.Empty;
            MaxLength = 1000;
            Files.Clear();
        }

        public void ToCommentState(StatusModel model)
        {
            IsSending = false;
            PostType = PostType.Comment;
            StatusModel = model;
            MaxImageFileCount = 1;
            Content = string.Empty;
            MaxLength = 140;
            Files.Clear();
        }

        public void ToRepostState(StatusModel model)
        {
            IsSending = false;
            PostType = PostType.Repost;
            StatusModel = model.RetweetedStatus ?? model;
            MaxImageFileCount = 1;
            Content = model.RetweetedStatus == null ? string.Empty : model.RawText;
            MaxLength = 140;
            Files.Clear();
        }

        public async Task Commit()
        {
            var textLength = Encoding.GetEncoding("gb2312").GetByteCount(Content) / 2d;
            if (textLength > MaxLength || string.IsNullOrEmpty(Content))
            {
                //TODO: notify text out of range
                return;
            }

            IsSending = true;
            var picids = await Task.WhenAll(Files.Select(async it => await Singleton<Api>.Instance.UploadPic(it)));
            WeiboResponse<JObject> result;
            switch (PostType)
            {
                case PostType.Create:
                {
                    result = await Singleton<Api>.Instance.Update(Content, picids.Select(it => it.PicId).ToArray());
                }
                    break;
                case PostType.Repost:
                {
                    result = await Singleton<Api>.Instance.Repost(Content, StatusModel, picids.FirstOrDefault()?.PicId);
                }
                    break;
                case PostType.Comment:
                {
                    result =
                        await Singleton<Api>.Instance.Comment(Content, StatusModel, picids.FirstOrDefault()?.PicId);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            IsSending = false;
            if (result.Ok != 1)
            {
                //TODO: notify send error
                Debug.WriteLine($"Send error: {result.Data}");
            }
        }
    }
}