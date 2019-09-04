using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class StatusViewModel : ViewModelBase
    {
        public StatusViewModel(StatusModel status)
        {
            Status = status;
            Init();
        }

        public StatusModel Status { get; }

        private async void Init()
        {
            if (Status.IsLongText)
            {
                long.TryParse(Status.Id, out var id);
                var result = await Singleton<Api>.Instance.Extend(id);
                Status.Text = result.LongTextContent;
                OnPropertyChanged(nameof(Status));
            }
        }
    }
}