using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class StoryBlogArticle
    {
        public int StoryId { get; set; }
        public int BlogArticleId { get; set; }

        public virtual BlogArticle BlogArticle { get; set; }
        public virtual Story Story { get; set; }
    }
}
