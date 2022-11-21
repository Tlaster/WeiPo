namespace WeiPoX.Paging;

public record PagingConfig(uint PageSize, uint InitialLoadSize, uint PrefetchDistance, int MaxSize = int.MaxValue)
{
    public PagingConfig(uint pageSize) : this(pageSize, pageSize * 3, pageSize)
    {
    }
}