using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nito.Mvvm;
using WeiPo.Common;
using WeiPo.Services;
using WeiPo.Services.Models;

namespace WeiPo.ViewModels
{
    public class DockViewModel : ViewModelBase
    {
        private DockViewModel()
        {
        }

        public static DockViewModel Instance { get; } = new DockViewModel();

        public PostWeiboViewModel PostWeiboViewModel { get; } = new PostWeiboViewModel();

        public Lazy<NotifyTask<IEnumerable<IGrouping<string, EmojiModel>>>> Emoji { get; } = new Lazy<NotifyTask<IEnumerable<IGrouping<string, EmojiModel>>>>(() => NotifyTask.Create(LoadEmoji));

        private static async Task<IEnumerable<IGrouping<string, EmojiModel>>> LoadEmoji()
        {
            var emojiResponse = await Singleton<Api>.Instance.Emoji();
            var emojis = new List<EmojiModel>();
            emojis.AddRange(emojiResponse.Data.Usual.SelectMany(it => it.Value));
            emojis.AddRange(emojiResponse.Data.More.SelectMany(it => it.Value));
            emojis.AddRange(emojiResponse.Data.Brand.Norm.SelectMany(it => it.Value));
            return emojis.GroupBy(it => it.Category);
        }
    }
}