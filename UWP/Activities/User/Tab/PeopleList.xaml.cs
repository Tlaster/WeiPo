using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using WeiPo.Common;
using WeiPo.Services.Models;
using WeiPo.ViewModels.User.Tab;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Activities.User.Tab
{
    public sealed partial class PeopleList : INotifyPropertyChanged
    {
        public enum ListType
        {
            Follow,
            Fans
        }

        public static readonly DependencyProperty TabDataProperty = DependencyProperty.Register(
            nameof(TabData), typeof(Services.Models.Tab), typeof(PeopleList),
            new PropertyMetadata(default(Services.Models.Tab), PropertyChangedCallback));

        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(
            nameof(Type), typeof(ListType), typeof(PeopleList), new PropertyMetadata(default(ListType)));

        public PeopleList()
        {
            InitializeComponent();
        }

        public PeopleListViewModel ViewModel { get; private set; }

        public Services.Models.Tab TabData
        {
            get => (Services.Models.Tab) GetValue(TabDataProperty);
            set => SetValue(TabDataProperty, value);
        }

        public ListType Type
        {
            get => (ListType) GetValue(TypeProperty);
            set => SetValue(TypeProperty, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == TabDataProperty) (d as PeopleList).OnTabDataChanged();
        }

        private void OnTabDataChanged()
        {
            var uid = long.Parse(TabData.Containerid);
            ViewModel = new PeopleListViewModel(Type, uid);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is UserModel user)
                Singleton<MessagingCenter>.Instance.Send(this, "user_clicked", user);
        }
    }
}