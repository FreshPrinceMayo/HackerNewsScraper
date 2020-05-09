using HackerNewsScraper.HackerNewApi;
using HackerNewsScraper.Models;
using HackerNewsScraper.Services;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace HackerNewsScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Processing");

            //new GetStories().Execute("topstories.json");
            //new GetStories().Execute("beststories.json");
            //new GetStories().Execute("newstories.json");

            //new ExtractBlogData().Execute();

            Console.WriteLine("Finishing Processing");
        }

        

    }




}
