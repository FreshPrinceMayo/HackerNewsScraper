using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class Story
    {
        public Story()
        {
            Comment = new HashSet<Comment>();
            StoryBlog = new HashSet<StoryBlog>();
        }

        public long StoryId { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public int? Score { get; set; }
        public int? Descendants { get; set; }
        public long? Time { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<StoryBlog> StoryBlog { get; set; }
    }
}
