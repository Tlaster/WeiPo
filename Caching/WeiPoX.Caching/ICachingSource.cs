namespace WeiPoX.Caching;

public interface ICachingSource<TKey, TValue>
{
    IObservable<TValue> Load(TKey key, CancellationToken cancellationToken);
}
