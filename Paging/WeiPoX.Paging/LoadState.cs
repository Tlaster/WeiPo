using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;

namespace WeiPoX.Paging;

public interface ILoadState
{
    bool EndOfPaginationReached { get; }
}

public static class LoadState
{
    public static LoadingImpl Loading { get; } = new();

    public record NotLoading(bool EndOfPaginationReached) : ILoadState
    {
        public static NotLoading InComplete { get; } = new(false);
        public static NotLoading Complete { get; } = new(true);
    }

    public class LoadingImpl : ILoadState
    {
        internal LoadingImpl()
        {
        }

        public bool EndOfPaginationReached => false;
    }

    public record Error(Exception Exception) : ILoadState
    {
        public bool EndOfPaginationReached => false;
    }
}

public static class LoadStateExtension
{
    public static bool IsInComplete(this ILoadState state)
    {
        return state is LoadState.NotLoading
        {
            EndOfPaginationReached: false
        };
    }

    public static bool IsComplete(this ILoadState state)
    {
        return state is LoadState.NotLoading
        {
            EndOfPaginationReached: true
        };
    }
}

public record LoadStates(
    ILoadState Refresh,
    ILoadState Append
);

public class MutableLoadStateCollection
{
    private readonly Subject<LoadStates?> _loadStatesSubject = new();

    private LoadStates _loadStates = new(
        LoadState.NotLoading.InComplete,
        LoadState.NotLoading.InComplete
    );

    public IObservable<LoadStates> LoadStates => _loadStatesSubject.WhereNotNull().AsObservable();

    public void Set(LoadType type, ILoadState state)
    {
        _loadStates = type switch
        {
            LoadType.Refresh => _loadStates with { Refresh = state },
            LoadType.Append => _loadStates with { Append = state },
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
        _loadStatesSubject.OnNext(_loadStates);
    }

    public ILoadState GetRefresh()
    {
        return _loadStates.Refresh;
    }

    public ILoadState GetAppend()
    {
        return _loadStates.Append;
    }
}

public record LoadKeyHolder<T>(T? Refresh, T? Append);