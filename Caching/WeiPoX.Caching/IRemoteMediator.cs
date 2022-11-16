using CSharpFunctionalExtensions;

namespace WeiPoX.Caching;

public interface IRemoteMediator
{
    Task<Result> RefreshAsync();
}