namespace WeiPoX.Caching;

public interface IRemoteMediator
{
    
}

public interface ILoadResult
{
}

public static class LoadResult
{
    public record Error(Exception Exception) : ILoadResult;

    public record Result<TValue>(TValue Data) : ILoadResult;
}