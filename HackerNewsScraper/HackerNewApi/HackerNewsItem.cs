using System;
using System.Collections.Generic;
using System.Text;

namespace HackerNewsScraper.HackerNewApi
{
    public class HackerNewsItem
    {
        public int id { get; set; }
        public string text { get; set; }
        public string by { get; set; }
        public int? parent { get; set; }
        public List<int> kids { get; set; }
        public string type { get; set; }
        public long time { get; set; }
    }
}
