using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class CommentText
    {
        public long CommentId { get; set; }
        public string Text { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
