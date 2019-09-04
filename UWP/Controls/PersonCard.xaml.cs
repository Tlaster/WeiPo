using Windows.UI.Xaml;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Controls
{
    public sealed partial class PersonCard
    {
        public static readonly DependencyProperty AvatarProperty = DependencyProperty.Register(
            nameof(Avatar), typeof(string), typeof(PersonCard), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty SubTitleProperty = DependencyProperty.Register(
            nameof(SubTitle), typeof(string), typeof(PersonCard), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            nameof(Title), typeof(string), typeof(PersonCard), new PropertyMetadata(default(string)));

        public PersonCard()
        {
            InitializeComponent();
        }

        public string SubTitle
        {
            get => (string) GetValue(SubTitleProperty);
            set => SetValue(SubTitleProperty, value);
        }

        public string Title
        {
            get => (string) GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public string Avatar
        {
            get => (string) GetValue(AvatarProperty);
            set => SetValue(AvatarProperty, value);
        }
    }
}