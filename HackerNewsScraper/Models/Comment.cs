using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class Comment
    {
        public long CommentId { get; set; }
        public long StoryId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Story Story { get; set; }
    }
}
