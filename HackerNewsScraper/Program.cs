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


         
            new GetStories().Execute("topstories.json");
            new GetStories().Execute("beststories.json");
            new GetStories().Execute("newstories.json");

            //new GetStoriesAlgolia().Execute("opinion",true);
            //new GetStoriesAlgolia().Execute("thoughts", true);


            new ExtractBlogData().Execute();
            new ExtractBlogData().Execute();
            new LinkStoryToBlogArticle().Execute();



            using (var context = new HackerNewsContext())
            {
                var storyIds = context.Story.Where(x => !x.StoryProcessed.Any() && x.StoryBlog.Any()).Select(x => x.StoryId).ToList();

                foreach (var storyId in storyIds)
                {
                    Console.WriteLine($"Getting comment from story {storyId}");
                    new GetComments().Execute(storyId);
                }

            }


            new ProcessComments().Execute();
            new ProcessCommentsBlogs().Excute();
            new ExtractBlogData().Execute();



            Console.WriteLine("Finishing Processing");
        }

        

    }




}
