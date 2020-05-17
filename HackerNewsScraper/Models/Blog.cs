using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class Blog
    {
        public Blog()
        {
            BlogArticle = new HashSet<BlogArticle>();
            StoryBlog = new HashSet<StoryBlog>();
        }

        public int BlogId { get; set; }
        public string BaseUrl { get; set; }
        public bool? IsCompany { get; set; }
        public string AuthorName { get; set; }
        public string Twitter { get; set; }

        public virtual ICollection<BlogArticle> BlogArticle { get; set; }
        public virtual ICollection<StoryBlog> StoryBlog { get; set; }
    }
}
