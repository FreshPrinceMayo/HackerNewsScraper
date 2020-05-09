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
    public class GetStories
    {
        public void Execute(string api)
        {
            var storyIds = new List<long>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(api).Result;
                if (response.IsSuccessStatusCode)
                {
                    storyIds = JsonSerializer.Deserialize<List<long>>(response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }

                var existingStories = new HackerNewsContext().Story.Select(x => x.StoryId).ToList();


                //var numberList = Enumerable.Range(22126484, 23126484).ToList();

                foreach (var storyId in storyIds.Except(existingStories))
                {
                    var story = new HackerNewsStory();

                    HttpResponseMessage responseStory = client.GetAsync($"item/{storyId}.json").Result;
                    if (responseStory.IsSuccessStatusCode)
                    {
                        story = JsonSerializer.Deserialize<HackerNewsStory>(responseStory.Content.ReadAsStringAsync().Result);

                        if(story == null)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Internal server Error");
                    }

                    try
                    {
                        var currentDateTime = DateTime.Now;


                        using (var context = new HackerNewsContext())
                        {
                            var newStory = new Story
                            {
                                StoryId = story.id,
                                Title = story.title,
                                CreatedBy = story.by,
                                Type = story.type,
                                Url = story.url,
                                Score = story.score,
                                Descendants = story.descendants,
                                Time = story.time,
                                CreatedDate = currentDateTime
                            };

                            if (story.kids != null && story.kids.Any())
                            {
                                foreach (var commentId in story.kids)
                                {
                                    newStory.Comment.Add(new Comment
                                    {
                                        CommentId = commentId,
                                        CreatedDate = currentDateTime
                                    });
                                }
                            }


                            context.Story.Add(newStory);
                            context.SaveChanges();
                            Console.WriteLine($"Added Story {newStory.Title}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

            }
        }
    }
}
