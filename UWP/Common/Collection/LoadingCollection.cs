using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;

namespace WeiPo.Common.Collection
{
    public interface ISupportRefresh
    {
        Task Refresh();
    }

    public class LoadingCollection<TSource, IType> : IncrementalLoadingCollection<TSource, IType>, ISupportRefresh
        where TSource : IIncrementalSource<IType>
    {
        public LoadingCollection(int itemsPerPage = 20, Action onStartLoading = null, Action onEndLoading = null,
            Action<Exception> onError = null) : base(itemsPerPage, onStartLoading, onEndLoading, onError)
        {
        }

        public LoadingCollection(TSource source, int itemsPerPage = 20, Action onStartLoading = null,
            Action onEndLoading = null, Action<Exception> onError = null) : base(source, itemsPerPage, onStartLoading,
            onEndLoading, onError)
        {
        }

        public async Task Refresh()
        {
            Clear();
            await RefreshAsync();
        }
    }
}