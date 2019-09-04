using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace WeiPo.Controls
{
    public sealed class ChipView : Control
    {
        public static readonly DependencyProperty CloseButtonVisibilityProperty = DependencyProperty.Register(
            nameof(CloseButtonVisibility), typeof(Visibility), typeof(ChipView),
            new PropertyMetadata(Visibility.Visible));

        public static readonly DependencyProperty ShadowOpacityProperty = DependencyProperty.Register(
            nameof(ShadowOpacity), typeof(double), typeof(ChipView), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty SymbolContentBackgroundProperty = DependencyProperty.Register(
            nameof(SymbolContentBackground), typeof(Brush), typeof(ChipView), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty SymbolContentProperty = DependencyProperty.Register(
            nameof(SymbolContent), typeof(object), typeof(ChipView), new PropertyMetadata(default));

        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
            nameof(Symbol), typeof(Symbol), typeof(ChipView), new PropertyMetadata(default(Symbol)));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            nameof(Text), typeof(string), typeof(ChipView), new PropertyMetadata(default(string)));

        private Button _closeButton;

        public ChipView()
        {
            DefaultStyleKey = typeof(ChipView);
        }

        public double ShadowOpacity
        {
            get => (double) GetValue(ShadowOpacityProperty);
            set => SetValue(ShadowOpacityProperty, value);
        }

        public Visibility CloseButtonVisibility
        {
            get => (Visibility) GetValue(CloseButtonVisibilityProperty);
            set => SetValue(CloseButtonVisibilityProperty, value);
        }

        public string Text
        {
            get => (string) GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Brush SymbolContentBackground
        {
            get => (Brush) GetValue(SymbolContentBackgroundProperty);
            set => SetValue(SymbolContentBackgroundProperty, value);
        }

        public object SymbolContent
        {
            get => GetValue(SymbolContentProperty);
            set => SetValue(SymbolContentProperty, value);
        }

        public Symbol Symbol
        {
            get => (Symbol) GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }

        public event EventHandler CloseRequest;


        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _closeButton = GetTemplateChild("CloseButton") as Button;
            if (_closeButton != null)
            {
                _closeButton.Tapped += CloseButtonOnTapped;
            }
        }

        private void CloseButtonOnTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}