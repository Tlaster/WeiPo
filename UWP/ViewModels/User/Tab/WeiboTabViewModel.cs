using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Collections;
using Microsoft.Toolkit.Uwp;
using Newtonsoft.Json.Linq;
using WeiPo.Common;
using WeiPo.Common.Collection;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User.Tab
{
    public class InterestPeopleViewModel
    {
        public InterestPeopleViewModel(List<InterestPeopleModel> items, InterestPropleDescModel descModel)
        {
            Items = items;
            DescModel = descModel;
        }

        public InterestPropleDescModel DescModel { get; }  
        public List<InterestPeopleModel> Items { get; }

        public void OnItemClicked(object sender, TappedRoutedEventArgs args)
        {
            if (args.OriginalSource is FrameworkElement element && element.DataContext is InterestPeopleModel model)
            {
                Singleton<MessagingCenter>.Instance.Send(this, "user_clicked", model.User.Id);
            }
        }
    }

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
                            return new InterestPeopleViewModel(obj["card_group"].Skip(1)
                                .Select(card => card.ToObject<InterestPeopleModel>()).ToList(), obj["card_group"].FirstOrDefault()?.ToObject<InterestPropleDescModel>()) as object;

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
                new LoadingCollection<WeiboTabDataSource, object>(
                    new WeiboTabDataSource(profile.UserInfo.Id, tabData.Containerid));
        }

        public LoadingCollection<WeiboTabDataSource, object> DataSource { get; }
    }
}