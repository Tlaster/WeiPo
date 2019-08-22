using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User.Tab
{
    public class AlbumTabViewModel : AbsTabViewModel
    {
        public AlbumTabViewModel(ProfileData profile, Services.Models.Tab tabData) : base(profile, tabData)
        {
        }
    }
}
