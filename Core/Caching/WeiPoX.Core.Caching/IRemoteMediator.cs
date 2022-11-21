using CSharpFunctionalExtensions;

namespace WeiPoX.Core.Caching;

public interface IRemoteMediator
{
    Task<Result> RefreshAsync();
}