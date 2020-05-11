using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace HackerNewsScraper.HackerNewApi
{
    public class HackerNewsAlgoliaStory
    {
        public int page { get; set; }
        public int nbHits { get; set; }
        public int nbPages { get; set; }
        public List<SearchHits> hits { get; set; }

    }

    public class SearchHits
    {
        public string title { get; set; }
        public string url { get; set; }
        public string author { get; set; }
        public int points { get; set; }
        public string objectID { get; set; }
    }
}

