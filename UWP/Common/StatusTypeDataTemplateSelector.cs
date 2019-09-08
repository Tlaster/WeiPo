using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WeiPo.Services.Models;

namespace WeiPo.Common
{
    public class StatusTypeDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate StatusTemplate { get; set; }
        public DataTemplate CommentTemplate { get; set; }
        public DataTemplate AttitudeTemplate { get; set; }
        public DataTemplate MessageListTemplate { get; set; }

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
                case AttitudeModel attitude:
                    return AttitudeTemplate;
                case MessageListModel message:
                    return MessageListTemplate;
            }

            return new DataTemplate();
        }
    }
}