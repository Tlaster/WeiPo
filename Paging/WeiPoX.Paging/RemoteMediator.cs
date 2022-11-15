namespace WeiPoX.Paging;

public abstract class RemoteMediator<TKey, TValue>
{
    public virtual InitializeAction Initialize { get; } = InitializeAction.LaunchInitialRefresh;

    public abstract Task<IMediatorResult> Load(LoadType loadType, TKey key, int pageSize);
}

public interface IMediatorResult
{
}

public record Error(Exception Exception) : IMediatorResult;

public record Success(bool EndOfPaginationReached) : IMediatorResult;

public enum InitializeAction
{
    LaunchInitialRefresh,
    SkipInitialRefresh
}

public enum LoadType
{
    Refresh,
    Append
}