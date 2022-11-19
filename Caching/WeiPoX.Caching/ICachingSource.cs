namespace WeiPoX.Caching;

public interface ICachingSource<TKey, TValue>
{
    IObservable<TValue> Get(TKey key);
}