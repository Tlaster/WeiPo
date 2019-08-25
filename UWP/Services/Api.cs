using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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
                .GetJsonAsync<WeiboResponse<TimelineData>>(cancellationToken: cancellationToken);

        public async Task<WeiboResponse<ProfileData>> Profile(long id) =>
            await $"{HOST}/api/container/getIndex".SetQueryParams(new
                {
                    type = "uid",
                    value = id,
                })
                .WithCookies(GetCookies())
                .GetJsonAsync<WeiboResponse<ProfileData>>();

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
                .GetJsonAsync<WeiboResponse<JObject>>();

        public async Task<WeiboResponse<ProfileData>> Me()
        {
            var result = await $"{HOST}/api/config".WithCookies(GetCookies()).GetJsonAsync<WeiboResponse<JObject>>();
            var uidStr = result.Data.Value<string>("uid");
            long.TryParse(uidStr, out var uid);
            return await Profile(uid);
        }

        private Dictionary<string, string> GetCookies()
        {
            return Singleton<Storage>.Instance.Load("usercookie", string.Empty).FromJson<Dictionary<string, string>>();
        }
    }
}
