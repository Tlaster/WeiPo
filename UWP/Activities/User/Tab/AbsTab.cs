using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using WeiPo.Services.Models;
using WeiPo.ViewModels.User.Tab;

namespace WeiPo.Activities.User.Tab
{
    public abstract class AbsTab : UserControl, INotifyPropertyChanged
    {
        public AbsTabViewModel ViewModel
        {
            get => _viewModel;
            private set
            {
                _viewModel = value;
                OnPropertyChanged();
            }
        }

        public static readonly DependencyProperty TabDataProperty = DependencyProperty.Register(
            nameof(TabData), typeof(Services.Models.Tab), typeof(AbsTab), new PropertyMetadata(default, PropertyChangedCallback));

        private AbsTabViewModel _viewModel;

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == TabDataProperty)
            {
                (d as AbsTab).OnTabDataChanged();
            }
        }

        public AbsTab()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            var user = this.FindAscendant<UserActivity>();
            if (user != null)
            {
                ViewModel = CreateViewModel(user.ViewModel.Profile, TabData);
            }
        }

        private void OnTabDataChanged()
        {
        }

        public Services.Models.Tab TabData
        {
            get => (Services.Models.Tab) GetValue(TabDataProperty);
            set => SetValue(TabDataProperty, value);
        }

        protected abstract AbsTabViewModel CreateViewModel(ProfileData viewModelProfile, Services.Models.Tab tabData);
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}