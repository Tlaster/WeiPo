using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WeiPo.Services.Models;

namespace WeiPo.Common
{
    public class StatusCommentDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StatusTemplate { get; set; }
        public DataTemplate CommentTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return SelectTemplateCore(item, null);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            switch (item)
            {
                case StatusModel status:
                    return StatusTemplate;
                case CommentModel comment:
                    return CommentTemplate;
            }

            return new DataTemplate();
        }
    }
}