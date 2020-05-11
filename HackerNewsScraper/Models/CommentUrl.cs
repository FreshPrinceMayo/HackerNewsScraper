using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class CommentUrl
    {
        public int CommentUrlId { get; set; }
        public long CommentId { get; set; }
        public string Url { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
