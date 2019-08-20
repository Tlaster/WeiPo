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
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class TimelineDataSource : IIncrementalSource<StatusModel>
    {
        private long _maxId = 0;
        public async Task<IEnumerable<StatusModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            if (pageIndex == 0)
            {
                _maxId = 0;
            }
            var result = await Singleton<Api>.Instance.Timeline(_maxId, cancellationToken);
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
