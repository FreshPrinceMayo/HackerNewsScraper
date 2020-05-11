﻿using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class Comment
    {
        public Comment()
        {
            CommentUrl = new HashSet<CommentUrl>();
        }

        public long CommentId { get; set; }
        public long StoryId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? ParentCommentId { get; set; }
        public long? SubmittedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool? HasLink { get; set; }

        public virtual Story Story { get; set; }
        public virtual CommentText CommentText { get; set; }
        public virtual ICollection<CommentUrl> CommentUrl { get; set; }
    }
}
