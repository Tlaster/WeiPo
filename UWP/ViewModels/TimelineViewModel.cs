using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using WeiPo.Common;
using WeiPo.Common.Collection;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class TimelineDataSource : IIncrementalSource<StatusModel>
    {
        private long _maxId;

        public async Task<IEnumerable<StatusModel>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (pageIndex == 0)
            {
                _maxId = 0;
            }

            var result = await Singleton<Api>.Instance.Timeline(_maxId, cancellationToken);
            var list = result.Statuses;
            _maxId = result.MaxId;
            return list;
        }
    }

    public class TimelineViewModel : ViewModelBase
    {
        public LoadingCollection<TimelineDataSource, StatusModel> Timeline { get; } =
            new LoadingCollection<TimelineDataSource, StatusModel>();
    }
}