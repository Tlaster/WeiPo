using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using WeiPo.Common;
using WeiPo.Controls.Html;
using WeiPo.Services.Models;
using WeiPo.ViewModels;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace WeiPo.Controls
{
    internal static class CommentViewXamlHelper
    {
        public static Visibility PicVisibility(CommentModel comment)
        {
            return comment?.Pic?.Url == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }

    public sealed partial class CommentView
    {
        public static readonly DependencyProperty CommentProperty = DependencyProperty.Register(
            nameof(Comment), typeof(CommentModel), typeof(CommentView), new PropertyMetadata(default(CommentModel)));

        public static readonly DependencyProperty ShowActionsProperty = DependencyProperty.Register(
            nameof(ShowActions), typeof(bool), typeof(CommentView), new PropertyMetadata(true));

        public static readonly DependencyProperty ShowStatusProperty = DependencyProperty.Register(
            nameof(ShowStatus), typeof(bool), typeof(CommentView), new PropertyMetadata(true));

        public CommentView()
        {
            InitializeComponent();
        }

        public bool ShowStatus
        {
            get => (bool) GetValue(ShowStatusProperty);
            set => SetValue(ShowStatusProperty, value);
        }

        public bool ShowActions
        {
            get => (bool) GetValue(ShowActionsProperty);
            set => SetValue(ShowActionsProperty, value);
        }

        public CommentModel Comment
        {
            get => (CommentModel) GetValue(CommentProperty);
            set => SetValue(CommentProperty, value);
        }

        private void CommentTapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
            Singleton<BroadcastCenter>.Instance.Send(this, "status_comment", Comment);
        }

        private void HtmlTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            WeiboLinkHelper.LinkClicked(e.Link);
        }

        private void ImageEx_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Comment.Pic == null)
            {
                return;
            }

            e.Handled = true;
            Singleton<BroadcastCenter>.Instance.Send(this, "image_clicked",
                new ImageViewModel(new[]
                {
                    new ImageModel(Comment.Pic.Url, Comment.Pic.Large.Url, Comment.Pic.Large.Geo.Width,
                        Comment.Pic.Large.Geo.Height)
                }));
        }

        private void LikeTapped(object sender, TappedRoutedEventArgs e)
        {
        }
    }
}