using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using WeiPo.Common;
using WeiPo.Services.Models;

#nullable enable
namespace WeiPo.Services
{
    internal static class ApiExtensions
    {
        public static async Task<T> GetData<T>(this Task<WeiboResponse<T>> task) where T : class
        {
            try
            {
                var result = await task;
                return result.Data;
            }
            catch (WeiboException e)
            {
                //todo: show notification
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }

                throw;
            }
        }
    }

    internal class Api
    {
        public const string HOST = "https://m.weibo.cn";

        public async Task<JObject> Comment(string content, ICanReply status, string? picId = null)
        {
            var configResult = await Config();
            return await $"{HOST}/api/comments/create"
                .WithHeader("Referer",
                    $"{HOST}/compose/comment?id={status.Id}{(string.IsNullOrEmpty(picId) ? "" : $"&pids={picId}")}")
                .PostUrlEncodedAsync(new
                {
                    id = status.Id,
                    mid = status.Mid,
                    content,
                    st = configResult.St,
                    picId
                })
                .ReceiveJson<WeiboResponse<JObject>>()
                .GetData();
        }

        public async Task<EmojiResponse> Emoji()
        {
            return await "https://weibo.com/aj/mblog/face?type=face"
                .GetAsync()
                .ReceiveJson<EmojiResponse>();
        }

        public async Task<JObject> Reply(string content, CommentModel item, string picId = null)
        {
            var configResult = await Config();
            return await $"{HOST}/api/comments/reply"
                .WithHeader("Referer",
                    $"{HOST}/compose/reply?id={item.Id}{(string.IsNullOrEmpty(picId) ? "" : $"&pids={picId}")}")
                .PostUrlEncodedAsync(new
                {
                    id = item.Status.Id,
                    mid = item.Status.Id,
                    content,
                    cid = item.Id,
                    reply = item.Id,
                    st = configResult.St,
                    picId
                })
                .ReceiveJson<WeiboResponse<JObject>>()
                .GetData();
        }

        public async Task<ConfigModel> Config()
        {
            return await $"{HOST}/api/config"
                .GetAsync()
                .ReceiveJson<WeiboResponse<ConfigModel>>()
                .GetData();
        }

        public async Task<LongTextModel> Extend(long id)
        {
            return await $"{HOST}/statuses/extend"
                .SetQueryParams(new
                {
                    id
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<LongTextModel>>()
                .GetData();
        }

        public async Task<JObject> Fans(long uid, int page = 1)
        {
            var info = await $"{HOST}/profile/info"
                .SetQueryParams(new
                {
                    uid
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<JObject>>();
            var container = info.Data.Value<string>("fans");
            container = container.Substring(container.IndexOf('?') + 1);
            return await $"{HOST}/api/container/getSecond?{container}"
                .SetQueryParam("page", page)
                .GetAsync()
                .ReceiveJson<WeiboResponse<JObject>>()
                .GetData();
        }

        public async Task<JObject> Follow(long uid, int page = 1)
        {
            var info = await $"{HOST}/profile/info"
                .SetQueryParams(new
                {
                    uid
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<JObject>>();
            var container = info.Data.Value<string>("follow");
            container = container.Substring(container.IndexOf('?') + 1);
            return await $"{HOST}/api/container/getSecond?{container}"
                .SetQueryParam("page", page)
                .GetAsync()
                .ReceiveJson<WeiboResponse<JObject>>()
                .GetData();
        }

        public async Task<List<AttitudeModel>> GetAttitude(int page = 1)
        {
            return await $"{HOST}/message/attitude"
                .SetQueryParams(new
                {
                    page
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<List<AttitudeModel>>>()
                .GetData();
        }

        public async Task<List<CommentModel>> GetComment(int page = 1)
        {
            return await $"{HOST}/message/cmt"
                .SetQueryParams(new
                {
                    page
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<List<CommentModel>>>()
                .GetData();
        }


        public Dictionary<string, string> GetCookies()
        {
            return Singleton<Storage>.Instance.Load("usercookie", string.Empty).FromJson<Dictionary<string, string>>();
        }

        public async Task<List<StatusModel>> GetMentionsAt(int page = 1)
        {
            return await $"{HOST}/message/mentionsAt"
                .SetQueryParams(new
                {
                    page
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<List<StatusModel>>>()
                .GetData();
        }

        public async Task<List<CommentModel>> GetMentionsCmt(int page = 1)
        {
            return await $"{HOST}/message/mentionsCmt"
                .SetQueryParams(new
                {
                    page
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<List<CommentModel>>>()
                .GetData();
        }

        public async Task<List<MessageListModel>> GetMessageList(int page = 1)
        {
            return await $"{HOST}/message/msglist"
                .SetQueryParams(new
                {
                    page
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<List<MessageListModel>>>()
                .GetData();
        }

        public async Task<List<CommentModel>> GetMyComment(int page = 1)
        {
            return await $"{HOST}/message/myCmt"
                .SetQueryParams(new
                {
                    page
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<List<CommentModel>>>()
                .GetData();
        }

        public async Task<HotflowModel> Hotflow(long id, long mid, long max_id= 0)
        {
            return await $"{HOST}/comments/hotflow"
                .SetQueryParams(new
                {
                    id,
                    mid,
                    max_id,
                    max_id_type = 0
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<HotflowModel>>()
                .GetData();
        }

        public async Task<HotflowChildModel> HotflowChild(long cid, long max_id)
        {
            return await $"{HOST}/comments/hotFlowChild"
                .SetQueryParams(new
                {
                    cid,
                    max_id
                })
                .GetAsync()
                .ReceiveJson<HotflowChildModel>();
        }

        public async Task<ProfileData> Me()
        {
            var result = await Config();
            var uidStr = result.Uid;
            long.TryParse(uidStr, out var uid);
            return await Profile(uid);
        }

        public async Task<JObject> MyFans(int since_id = 0)
        {
            return await $"{HOST}/api/container/getIndex"
                .SetQueryParams(new
                {
                    containerid = "231016_-_selffans",
                    since_id
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<JObject>>()
                .GetData();
        }

        public async Task<ProfileData> Profile(long id)
        {
            return await $"{HOST}/api/container/getIndex".SetQueryParams(new
                {
                    type = "uid",
                    value = id
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<ProfileData>>()
                .GetData();
        }

        public async Task<ProfileData> Profile(string name)
        {
            return await Profile(await UserId(name));
        }

        public async Task<JObject> ProfileTab(long id, string containerid, long since_id = 0)
        {
            return await $"{HOST}/api/container/getIndex".SetQueryParams(new
                {
                    type = "uid",
                    value = id,
                    containerid,
                    since_id
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<JObject>>()
                .GetData();
        }

        public async Task<JObject> Repost(string content, ICanReply status, string picId = null)
        {
            var configResult = await Config();
            return await $"{HOST}/api/statuses/repost"
                .WithHeader("Referer",
                    $"{HOST}/compose/repost?id={status.Id}{(string.IsNullOrEmpty(picId) ? "" : $"&pids={picId}")}")
                .PostUrlEncodedAsync(new
                {
                    id = status.Id,
                    mid = status.Mid,
                    content,
                    st = configResult.St,
                    picId
                })
                .ReceiveJson<WeiboResponse<JObject>>()
                .GetData();
        }

        public async Task<RepostTimelineModel> RepostTimeline(long id, int page = 1)
        {
            return await $"{HOST}/api/statuses/repostTimeline"
                .SetQueryParams(new
                {
                    id,
                    page
                })
                .GetAsync()
                .ReceiveJson<WeiboResponse<RepostTimelineModel>>()
                .GetData();
        }

        public async Task<TimelineData> Timeline(long maxid = 0,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await $"{HOST}/feed/friends"
                .SetQueryParams(new
                {
                    max_id = maxid
                })
                .GetAsync(cancellationToken)
                .ReceiveJson<WeiboResponse<TimelineData>>()
                .GetData();
        }

        public async Task<UnreadModel> Unread()
        {
            return await $"{HOST}/api/remind/unread"
                .SetQueryParam("t", DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                .GetAsync()
                .ReceiveJson<WeiboResponse<UnreadModel>>()
                .GetData();
        }

        public async Task<JObject> Update(string content, params string[] pics)
        {
            var configResult = await Config();
            return await $"{HOST}/api/statuses/update"
                .WithHeader("Referer", $"{HOST}/compose/?{(!pics.Any() ? "" : $"&pids={string.Join(",", pics)}")}")
                .PostUrlEncodedAsync(new
                {
                    content,
                    st = configResult.St,
                    picId = string.Join(",", pics)
                })
                .ReceiveJson<WeiboResponse<JObject>>()
                .GetData();
        }

        public async Task<UploadPicModel> UploadPic(StorageFile file)
        {
            var configResult = await Config();
            var st = configResult.St;
            using var fileStream = await file.OpenStreamForReadAsync();

            return await $"{HOST}/api/statuses/uploadPic"
                .WithHeader("Referer", $"{HOST}/compose/")
                .PostMultipartAsync(it =>
                {
                    it.AddString("type", "json");
                    it.AddString("st", st);
                    it.AddFile("pic", fileStream, file.Name, file.ContentType);
                }).ReceiveJson<UploadPicModel>();
        }

        public async Task<long> UserId(string name)
        {
            using var client = new HttpClient();
            var response =
                await client.GetAsync($"{HOST}/n/{name}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var redirectUri = response.RequestMessage.RequestUri.ToString();
            var uidStr = Regex.Match(redirectUri, "\\/u\\/(\\d+)").Groups[1].Value;
            if (!long.TryParse(uidStr, out var uid))
            {
                throw new WeiboException("user not found");
            }
            return uid;
        }
    }
}