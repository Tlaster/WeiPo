using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using WeiPo.Common;
using WeiPo.Common.Collection;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User.Tab
{

    public class AlbumDataSource : IIncrementalSource<PicWall>
    {
        private readonly long _uid;
        private readonly string _containerId;

        public AlbumDataSource(string containerId, long uid)
        {
            _containerId = containerId;
            _uid = uid;
        }

        public async Task<IEnumerable<PicWall>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var result = await Singleton<Api>.Instance.PhotoAll(_uid, _containerId, pageIndex + 1);
                return result["cards"].SelectMany(it => it["pics"]).Select(it => it.ToObject<PicWall>());
            }
            catch (WeiboException e)
            {
                return new List<PicWall>();
            }
        }
    }

    public class AlbumTabViewModel : AbsTabViewModel
    {
        public AlbumTabViewModel(ProfileData profile, Services.Models.Tab tabData) : base(profile, tabData)
        {
            DataSource =
                new LoadingCollection<AlbumDataSource, PicWall>(new AlbumDataSource(tabData.Containerid,
                    profile.UserInfo.Id));
        }

        public LoadingCollection<AlbumDataSource, PicWall> DataSource { get; }
    }
}