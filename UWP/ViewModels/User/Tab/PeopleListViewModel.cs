using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using WeiPo.Activities.User.Tab;
using WeiPo.Common;
using WeiPo.Common.Collection;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User.Tab
{
    public class PeopleListDataSource : IIncrementalSource<UserModel>
    {
        private readonly PeopleList.ListType _type;
        private readonly long _uid;

        public PeopleListDataSource(PeopleList.ListType type, long uid)
        {
            _type = type;
            _uid = uid;
        }

        public async Task<IEnumerable<UserModel>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            switch (_type)
            {
                case PeopleList.ListType.Follow:
                {
                    var result = await Singleton<Api>.Instance.Follow(_uid, pageIndex + 1);
                    var items = result["cards"].Select(it => it["user"].ToObject<UserModel>()).ToList();
                    return items;
                }
                    break;
                case PeopleList.ListType.Fans:
                {
                    if (pageIndex == 0)
                    {
                        ////Check if user is me
                        //var uconfig = await Singleton<Api>.Instance.Config();
                        //long.TryParse(uconfig.Data.Uid, out var ucid);
                        //TODO:Not a good idea
                        var ucid = DockViewModel.Instance.MyProfile.Result.UserInfo.Id;
                        if (ucid == _uid)
                            //clear notification
                            await Singleton<Api>.Instance.MyFans();
                    }

                    var result = await Singleton<Api>.Instance.Fans(_uid, pageIndex + 1);
                    var items = result["cards"].Select(it => it["user"].ToObject<UserModel>()).ToList();
                    return items;
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public class PeopleListViewModel : ViewModelBase
    {
        public PeopleListViewModel(PeopleList.ListType listType, long uid)
        {
            ListType = listType;
            Uid = uid;
            Source = new LoadingCollection<PeopleListDataSource, UserModel>(new PeopleListDataSource(listType, uid));
        }

        public LoadingCollection<PeopleListDataSource, UserModel> Source { get; }

        public PeopleList.ListType ListType { get; }
        public long Uid { get; }
    }
}