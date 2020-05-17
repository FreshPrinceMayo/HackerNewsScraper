using System;
using System.Collections.Generic;

namespace HackerNewsScraper.Models
{
    public partial class StoryBlog
    {
        public int StoryId { get; set; }
        public int BlogId { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual Story Story { get; set; }
    }
}
