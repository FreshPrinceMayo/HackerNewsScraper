using System;
using System.Collections.Generic;
using System.Text;

namespace HackerNewsScraper.HackerNewApi
{
    public class HackerNewsItem
    {
        public long id { get; set; }
        public string text { get; set; }
        public string by { get; set; }
        public long? parent { get; set; }
        public List<long> kids { get; set; }
        public string type { get; set; }
        public long time { get; set; }
    }
}
