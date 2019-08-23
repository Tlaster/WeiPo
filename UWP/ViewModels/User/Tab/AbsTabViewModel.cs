using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels.User.Tab
{
    public abstract class AbsTabViewModel : ViewModelBase
    {
        public ProfileData ProfileData { get; set; }
        public Services.Models.Tab Tab { get; set; }
        protected AbsTabViewModel(ProfileData profile, Services.Models.Tab tabData)
        {
            ProfileData = profile;
            Tab = tabData;
        }

    }
}
