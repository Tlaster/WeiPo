using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace WeiPo.Services.Models
{
    public class PageInfo
    {
        [JsonProperty("object_type", NullValueHandling = NullValueHandling.Ignore)]
        public long ObjectType { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("page_pic", NullValueHandling = NullValueHandling.Ignore)]
        public PagePic PagePic { get; set; }

        [JsonProperty("page_url", NullValueHandling = NullValueHandling.Ignore)]
        public string PageUrl { get; set; }

        [JsonProperty("page_title", NullValueHandling = NullValueHandling.Ignore)]
        public string PageTitle { get; set; }

        [JsonProperty("content1", NullValueHandling = NullValueHandling.Ignore)]
        public string Content1 { get; set; }

        [JsonProperty("object_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ObjectId { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("content2", NullValueHandling = NullValueHandling.Ignore)]
        public string Content2 { get; set; }

        [JsonProperty("video_orientation", NullValueHandling = NullValueHandling.Ignore)]
        public string VideoOrientation { get; set; }

        [JsonProperty("play_count", NullValueHandling = NullValueHandling.Ignore)]
        public string PlayCount { get; set; }

        [JsonProperty("media_info", NullValueHandling = NullValueHandling.Ignore)]
        public MediaInfo MediaInfo { get; set; }

        [JsonProperty("urls", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Urls { get; set; }

        [JsonProperty("video_details", NullValueHandling = NullValueHandling.Ignore)]
        public VideoDetails VideoDetails { get; set; }

        [JsonProperty("slide_cover", NullValueHandling = NullValueHandling.Ignore)]
        public List<SlideCover> SlideCovers { get; set; }

        [JsonProperty("storyNumber", NullValueHandling = NullValueHandling.Ignore)]
        public long StoryNumber { get; set; }
    }

    public class SlideCover
    {
        [JsonProperty("nickname")]
        public string NickName { get; set; }
        [JsonProperty("pic")] 
        public string Pic { get; set; }
    }
}