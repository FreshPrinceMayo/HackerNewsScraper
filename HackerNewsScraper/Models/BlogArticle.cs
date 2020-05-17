using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class BlogArticle
    {
        public BlogArticle()
        {
            StoryBlogArticle = new HashSet<StoryBlogArticle>();
        }

        public int BlogArticleId { get; set; }
        public int? BlogId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public DateTime? PublishedDate { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual ICollection<StoryBlogArticle> StoryBlogArticle { get; set; }
    }
}
