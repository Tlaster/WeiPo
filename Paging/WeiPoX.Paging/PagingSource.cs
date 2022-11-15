namespace WeiPoX.Paging;

public interface IPagingSource<TKey, TValue>
{
    Task<ILoadResult> Load(ILoadParams<TKey> loadParams, CancellationToken cancellationToken);
}

public interface ILoadParams<T>
{
    uint LoadSize { get; }
    T? Key { get; }
}

public static class LoadParams
{
    public record Refresh<TKey>(uint LoadSize, TKey? Key = default) : ILoadParams<TKey>;

    public record Append<TKey>(uint LoadSize, TKey? Key) : ILoadParams<TKey>;
}

public interface ILoadResult
{
}

public static class LoadResult
{
    public record Error<TKey, TValue>(Exception Exception) : ILoadResult;

    public record Page<TKey, TValue>(IReadOnlyList<TValue> Data, TKey? NextKey) : ILoadResult;
}