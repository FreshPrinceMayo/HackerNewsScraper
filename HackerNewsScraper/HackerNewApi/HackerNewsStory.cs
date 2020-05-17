using System;
using System.Collections.Generic;
using System.Text;

namespace HackerNewsScraper.HackerNewApi
{
    public class HackerNewsStory
    {
        public int id { get; set; }
        public string title { get; set; }
        public string by { get; set; }
        public List<int> kids { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public int score { get; set; }
        public int descendants { get; set; }
        public long time { get; set; }
    }
}
