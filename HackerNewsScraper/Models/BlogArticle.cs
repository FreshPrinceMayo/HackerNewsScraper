using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class BlogArticle
    {
        public int BlogArticleId { get; set; }
        public int? BlogId { get; set; }
        public string Url { get; set; }

        public virtual Blog Blog { get; set; }
    }
}
