using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeiPo.Common;
using WeiPo.Services.Models;

namespace WeiPo.Services
{
    class Api
    {
        public const string HOST = "https://m.weibo.cn";

        public async Task<WeiboResponse<TimelineData>> Timeline(long maxid = 0,
            CancellationToken cancellationToken = new CancellationToken()) =>
            await $"{HOST}/feed/friends"
                .SetQueryParams(new
                {
                    max_id = maxid,
                })
                .WithCookies(GetCookies())
                .GetAsync(cancellationToken)
                .ReceiveJson<WeiboResponse<TimelineData>>();

        public async Task<WeiboResponse<ProfileData>> Profile(long id) =>
            await $"{HOST}/api/container/getIndex".SetQueryParams(new
                {
                    type = "uid",
                    value = id,
                })
                .WithCookies(GetCookies())
                .GetAsync()
                .ReceiveJson<WeiboResponse<ProfileData>>();

        public async Task<long> UserId(string name)
        {
            using var client = new HttpClient();
            var response =
                await client.GetAsync($"{HOST}/n/{name}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var redirectUri = response.RequestMessage.RequestUri.ToString();
            var uidStr = Regex.Match(redirectUri, "\\/u\\/(\\d+)").Groups[1].Value;
            long.TryParse(uidStr, out var uid);
            return uid;
        }

        public async Task<WeiboResponse<ProfileData>> Profile(string name) => await Profile(await UserId(name));

        public async Task<WeiboResponse<JObject>> ProfileTab(long id, string containerid, long since_id = 0) =>
            await $"{HOST}/api/container/getIndex".SetQueryParams(new
                {
                    type = "uid",
                    value = id,
                    containerid = containerid,
                    since_id = since_id,
                })
                .WithCookies(GetCookies())
                .GetAsync()
                .ReceiveJson<WeiboResponse<JObject>>();

        public async Task<WeiboResponse<ProfileData>> Me()
        {
            var result = await Config();
            var uidStr = result.Data.Uid;
            long.TryParse(uidStr, out var uid);
            return await Profile(uid);
        }

        public async Task<WeiboResponse<ConfigModel>> Config()
        {
            return await $"{HOST}/api/config".WithCookies(GetCookies()).GetAsync().ReceiveJson<WeiboResponse<ConfigModel>>();
        }

        public async Task<UploadPicModel> UploadPic(StorageFile file)
        {
            var configResult = await Config();
            var st = configResult.Data.St;
            using var fileStream = await file.OpenStreamForReadAsync();

            return await $"{HOST}/api/statuses/uploadPic"
                .WithHeader("Referer", $"{HOST}/compose/")
                .WithHeader("Cookie", string.Join(";", GetCookies().Select(it => $"{it.Key}={it.Value}")))
                .PostMultipartAsync(it =>
            {
                it.AddString("type", "json");
                it.AddString("st", st);
                it.AddFile("pic", fileStream, file.Name, file.ContentType);
            }).ReceiveJson<UploadPicModel>();
        }

        public async Task<WeiboResponse<JObject>> Update(string content, params string[] pics)
        {
            var configResult = await Config();
            return await $"{HOST}/api/statuses/update"
                .WithHeader("Referer", $"{HOST}/compose/?{(!pics.Any() ? "" : $"&pids={string.Join(",", pics)}" )}")
                .WithHeader("Cookie", string.Join(";", GetCookies().Select(it => $"{it.Key}={it.Value}")))
                .PostUrlEncodedAsync(new
            {
                content = content,
                st = configResult.Data.St,
                picId = string.Join(",", pics)
            }).ReceiveJson<WeiboResponse<JObject>>();
        }
        
        public async Task<WeiboResponse<JObject>> Repost(string content, StatusModel status, string picId = null)
        {
            var configResult = await Config();
            return await $"{HOST}/api/statuses/repost"
                .WithHeader("Referer", $"{HOST}/compose/repost?id={status.Id}{(string.IsNullOrEmpty(picId) ? "" : $"&pids={picId}" )}")
                .WithHeader("Cookie", string.Join(";", GetCookies().Select(it => $"{it.Key}={it.Value}")))
                .PostUrlEncodedAsync(new
            {
                id = status.Id,
                mid = status.Mid,
                content = content,
                st = configResult.Data.St,
                picId = picId
            }).ReceiveJson<WeiboResponse<JObject>>();
        }

        public async Task<WeiboResponse<JObject>> Comment(string content, StatusModel status, string picId = null)
        {
            var configResult = await Config();
            return await $"{HOST}/api/comments/create"
                .WithHeader("Referer", $"{HOST}/compose/comment?id={status.Id}{(string.IsNullOrEmpty(picId) ? "" : $"&pids={picId}" )}")
                .WithHeader("Cookie", string.Join(";", GetCookies().Select(it => $"{it.Key}={it.Value}")))
                .PostUrlEncodedAsync(new
            {
                id = status.Id,
                mid = status.Mid,
                content = content,
                st = configResult.Data.St,
                picId = picId
            }).ReceiveJson<WeiboResponse<JObject>>();
        }

        private Dictionary<string, string> GetCookies()
        {
            return Singleton<Storage>.Instance.Load("usercookie", string.Empty).FromJson<Dictionary<string, string>>();
        }
    }
}
