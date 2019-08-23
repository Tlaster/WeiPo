using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using Newtonsoft.Json.Linq;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User.Tab
{
    public class WeiboTabDataSource : IIncrementalSource<object>
    {
        private readonly string _containerId;

        private long _sinceId;
        private readonly long _userId;

        public WeiboTabDataSource(long userInfoId, string tabDataContainerid)
        {
            _userId = userInfoId;
            _containerId = tabDataContainerid;
        }

        public async Task<IEnumerable<object>> GetPagedItemsAsync(int pageIndex, int pageSize,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (pageIndex == 0) _sinceId = 0;

            var result = await Singleton<Api>.Instance.ProfileTab(_userId, _containerId, _sinceId);
            if (result.Ok == 1)
            {
                _sinceId = result.Data.SelectToken("cardlistInfo.since_id").Value<long>();
                var items = result.Data["cards"]
                    .Select(it =>
                    {
                        if (!(it is JObject obj)) return null;
                        if (obj.ContainsKey("mblog")) return obj["mblog"].ToObject<StatusModel>() as object;

                        if (obj.ContainsKey("itemid") && obj.Value<string>("itemid") == "INTEREST_PEOPLE")
                            return obj["card_group"].Skip(1)
                                .Select(card => card.ToObject<InterestPeopleModel>()) as object;

                        return null;
                    })
                    .Where(it => it != null);
                return items;
            }

            return new List<StatusModel>();
        }
    }

    public class WeiboTabViewModel : AbsTabViewModel
    {
        public WeiboTabViewModel(ProfileData profile, Services.Models.Tab tabData) : base(profile, tabData)
        {
            DataSource =
                new IncrementalLoadingCollection<WeiboTabDataSource, object>(
                    new WeiboTabDataSource(profile.UserInfo.Id, tabData.Containerid));
            DataSource.RefreshAsync().FireAndForget();
        }

        public IncrementalLoadingCollection<WeiboTabDataSource, object> DataSource { get; }
    }
}