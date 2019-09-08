

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

using WeiPo.Services.Models;
using WeiPo.ViewModels;

namespace WeiPo.Activities
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatusActivity
    {
        public StatusActivity()
        {
            InitializeComponent();
        }

        protected override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            switch (parameter)
            {
                case StatusModel status:
                    ViewModel = new StatusViewModel(status);
                    break;
            }

            DataContext = ViewModel;
        }

        public StatusViewModel ViewModel { get; private set; }
    }
}