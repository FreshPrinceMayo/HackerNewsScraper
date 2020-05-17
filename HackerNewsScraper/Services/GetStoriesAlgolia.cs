using HackerNewsScraper.HackerNewApi;
using HackerNewsScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace HackerNewsScraper.Services
{
    public class GetStoriesAlgolia
    {
        public void Execute(string query, bool latest = false)
        {
            var searchFilter = latest ? "search_by_date" : "search";
            var api = $"{searchFilter}?query={query}&tags=story";

            var currentDateTime = DateTime.Now;
            var storyIds = new List<int>();
            HttpResponseMessage response = new HttpResponseMessage();

            using (var context = new HackerNewsContext())
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://hn.algolia.com/api/v1/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var result = new HackerNewsAlgoliaStory();
                    response = client.GetAsync(api).Result;

                    storyIds = context.Story.Select(x => x.StoryId).ToList();


                    if (response.IsSuccessStatusCode)
                    {
                        result = JsonSerializer.Deserialize<HackerNewsAlgoliaStory>(response.Content.ReadAsStringAsync().Result);

                        foreach (var hit in result.hits)
                        {
                            if (storyIds.Any(x => x == Convert.ToInt32(hit.objectID)))
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine($"Story added already {hit.title}");
                                Console.ForegroundColor = ConsoleColor.White;
                                continue;
                            }


                            var newStory = new Story
                            {
                                StoryId = Convert.ToInt32(hit.objectID),
                                Title = string.IsNullOrWhiteSpace(hit.title) ? null : hit.title,
                                CreatedBy = hit.author,
                                Type = "story",
                                Url = string.IsNullOrWhiteSpace(hit.url) ? null : hit.url,
                                Score = hit.points,
                                CreatedDate = currentDateTime
                            };


                            context.Story.Add(newStory);
                            context.SaveChanges();

                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Added Story {newStory.Title}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Internal server Error");
                        Console.ForegroundColor = ConsoleColor.White;
                    }


                    for (int i = 1; i < result.nbPages; i++)
                    {
                        storyIds = context.Story.Select(x => x.StoryId).ToList();
                        response = client.GetAsync($"{api}&page={i}").Result;

                        if (response.IsSuccessStatusCode)
                        {
                            result = JsonSerializer.Deserialize<HackerNewsAlgoliaStory>(response.Content.ReadAsStringAsync().Result);


                            foreach (var hit in result.hits)
                            {
                                if (storyIds.Any(x => x == Convert.ToInt32(hit.objectID)))
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine($"Story added already {hit.title}");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    continue;
                                }


                                var newStory = new Story
                                {
                                    StoryId = Convert.ToInt32(hit.objectID),
                                    Title = string.IsNullOrWhiteSpace(hit.title) ? null : hit.title,
                                    CreatedBy = hit.author,
                                    Type = "story",
                                    Url = string.IsNullOrWhiteSpace(hit.url) ? null : hit.url,
                                    Score = hit.points,
                                    CreatedDate = currentDateTime
                                };


                                context.Story.Add(newStory);
                                context.SaveChanges();
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Added Story {newStory.Title}");
                                Console.ForegroundColor = ConsoleColor.White;
                            }


                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Internal server Error");
                            Console.ForegroundColor = ConsoleColor.White;
                        }


                    }


                }



            }
        }
    }
}
