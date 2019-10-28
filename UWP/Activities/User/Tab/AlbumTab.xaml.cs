using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using WeiPo.Common;
using WeiPo.Services.Models;
using WeiPo.ViewModels;
using WeiPo.ViewModels.User.Tab;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Activities.User.Tab
{
    public sealed partial class AlbumTab
    {
        public AlbumTab()
        {
            InitializeComponent();
        }

        protected override AbsTabViewModel CreateViewModel(ProfileData viewModelProfile, Services.Models.Tab tabData)
        {
            return new AlbumTabViewModel(viewModelProfile, tabData);
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            base.OnTapped(e);
            if (e.OriginalSource is FrameworkElement element && element.DataContext is PicWall model && ViewModel is AlbumTabViewModel viewModel)
            {
                e.Handled = true;
                var index = viewModel.DataSource.IndexOf(model);
                
                Singleton<BroadcastCenter>.Instance.Send(this, "image_clicked",
                    new ImageViewModel(viewModel.DataSource.Select(it => new ImageModel(it.PicMiddle, it.PicBig)).ToArray(), index));
            }
        }
    }
}