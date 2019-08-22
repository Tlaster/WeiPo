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
using WeiPo.Common;
using WeiPo.Services.Models;

namespace WeiPo.Services
{
    class Api
    {
        public async Task<WeiboResponse<TimelineData>> Timeline(long maxid = 0,
            CancellationToken cancellationToken = new CancellationToken()) =>
            await "https://m.weibo.cn/feed/friends"
                .SetQueryParams(new
                {
                    max_id = maxid,
                })
                .WithCookies(GetCookies())
                .GetJsonAsync<WeiboResponse<TimelineData>>(cancellationToken: cancellationToken);

        public async Task<WeiboResponse<ProfileData>> Profile(long id) =>
            await "https://m.weibo.cn/api/container/getIndex".SetQueryParams(new
                {
                    type = "uid",
                    value = id,
                })
                .WithCookies(GetCookies())
                .GetJsonAsync<WeiboResponse<ProfileData>>();


        public async Task<WeiboResponse<ProfileData>> Profile(string name)
        {
            using var client = new HttpClient();
            var response =
                await client.GetAsync($"https://m.weibo.cn/n/{name}", HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var redirectUri = response.RequestMessage.RequestUri.ToString();
            var uidStr = Regex.Match(redirectUri, "\\/u\\/(\\d+)").Groups[1].Value;
            long.TryParse(uidStr, out var uid);
            return await Profile(uid);
        }


        private Dictionary<string, string> GetCookies()
        {
            return Singleton<Storage>.Instance.Load("usercookie", string.Empty).FromJson<Dictionary<string, string>>();
        }
    }
}
