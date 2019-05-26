using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http.Filters;
using Flurl;
using Flurl.Http;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WeiPo.Common;

namespace WeiPo.ViewModels
{
    public class TimelineDataSource : IIncrementalSource<JToken>
    {
        private long _maxId = 0;
        public async Task<IEnumerable<JToken>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = (await "https://m.weibo.cn/feed/friends"
                .SetQueryParams(new
                {
                    max_id = _maxId,
                })
                .WithCookies(GetCookies())
                .GetStringAsync(cancellationToken: cancellationToken))
                .Let(JsonConvert.DeserializeObject<JObject>);
            if (result.Value<int>("ok") == 1)
            {
                var list = result["data"]["statuses"] as JArray;
                _maxId = result["data"]?.Value<long>("max_id") ?? 0L;
                return list;
            }
            else
            {
                return new List<JToken>();
            }
        }

        private Dictionary<string, string> GetCookies()
        {
            return Singleton<Storage>.Instance.Load("usercookie", string.Empty).FromJson<Dictionary<string, string>>();
        }
    }
    public class TimelineViewModel : ViewModelBase
    {
        public IncrementalLoadingCollection<TimelineDataSource, JToken> Timeline { get; } = new IncrementalLoadingCollection<TimelineDataSource, JToken>();

        public TimelineViewModel()
        {
            Timeline.RefreshAsync();
        }

    }
}
