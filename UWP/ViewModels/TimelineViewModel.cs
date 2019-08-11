﻿using System;
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
using WeiPo.Model;

namespace WeiPo.ViewModels
{
    public class TimelineDataSource : IIncrementalSource<StatusModel>
    {
        private long _maxId = 0;
        public async Task<IEnumerable<StatusModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = (await "https://m.weibo.cn/feed/friends"
                .SetQueryParams(new
                {
                    max_id = _maxId,
                })
                .WithCookies(GetCookies())
                .GetStringAsync(cancellationToken: cancellationToken))
                .Let(JsonConvert.DeserializeObject<StatusResponse>);
            
            if (result.Ok == 1)
            {
                var list = result.Data.Statuses;
                _maxId = result.Data.MaxId;
                return list;
            }
            else
            {
                return new List<StatusModel>();
            }
        }

        private Dictionary<string, string> GetCookies()
        {
            return Singleton<Storage>.Instance.Load("usercookie", string.Empty).FromJson<Dictionary<string, string>>();
        }
    }
    public class TimelineViewModel : ViewModelBase
    {
        public IncrementalLoadingCollection<TimelineDataSource, StatusModel> Timeline { get; } = new IncrementalLoadingCollection<TimelineDataSource, StatusModel>();

        public TimelineViewModel()
        {
            Timeline.RefreshAsync();
        }

    }
}
