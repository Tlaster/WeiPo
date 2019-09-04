

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace WeiPo.Activities
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VideoActivity
    {
        public VideoActivity()
        {
            InitializeComponent();
        }

        public string VideoPath { get; set; }

        protected override void OnCreate(object parameter)
        {
            base.OnCreate(parameter);
            if (parameter is string value)
            {
                VideoPath = value;
            }
        }
    }
}