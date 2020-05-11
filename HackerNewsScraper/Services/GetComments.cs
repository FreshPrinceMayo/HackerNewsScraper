using HackerNewsScraper.HackerNewApi;
using HackerNewsScraper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace HackerNewsScraper.Services
{
    public class GetComments
    {
        public void Execute(long storyId)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var now = DateTime.Now;
                var storyapi = new HackerNewsStory();

                HttpResponseMessage responseStory = client.GetAsync($"item/{storyId}.json").Result;
                if (responseStory.IsSuccessStatusCode)
                {
                    storyapi = JsonSerializer.Deserialize<HackerNewsStory>(responseStory.Content.ReadAsStringAsync().Result);

                    if (storyapi == new HackerNewsStory())
                    {
                        throw new Exception($"Can't find story {storyId}");
                    }
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }

                using (var context = new HackerNewsContext())
                {
                    var selectedStory = context.Story.Include(x => x.Comment).FirstOrDefaultAsync(x => x.StoryId == storyId).Result;

                    if (selectedStory == null)
                    {
                        throw new Exception($"Can't find story in database {storyId}");
                    }

                    selectedStory.Descendants = storyapi.descendants;
                    selectedStory.Score = storyapi.score;
                    selectedStory.Time = storyapi.time;

                    var currentCommentIds = selectedStory.Comment.Select(x => x.CommentId);

                    var newCommentIds = new List<long>();
                        
                    if(storyapi.kids != null)
                    {
                        newCommentIds.AddRange(storyapi.kids.Except(currentCommentIds));
                    }
                     

                    if (newCommentIds.Any())
                    {
                        foreach (var newCommentId in newCommentIds)
                        {
                            selectedStory.Comment.Add(new Comment
                            {
                                CommentId = newCommentId,
                                CreatedDate = now,
                                SubmittedDate = storyapi.time
                            });
                        }
                    }


                    context.Story.Update(selectedStory);
                    context.SaveChanges();

                    var storyCommentIds = new List<long>();

                    var existingComments = context.Comment.Include(x => x.CommentText).Where(x => x.StoryId == storyId);

                    storyCommentIds.AddRange(existingComments.Select(x => x.CommentId).ToList());


                    var hackerNewsItem = new HackerNewsItem();
                    for (int i = 0; i < storyCommentIds.Count; i++)
                    {
                        var selectedCommentId = storyCommentIds[i];

                        HttpResponseMessage responseComment = client.GetAsync($"item/{selectedCommentId}.json").Result;
                        if (responseComment.IsSuccessStatusCode)
                        {
                            hackerNewsItem = JsonSerializer.Deserialize<HackerNewsItem>(responseComment.Content.ReadAsStringAsync().Result);

                            if(hackerNewsItem == null)
                            {
                                continue;
                            }

                            if (storyapi == new HackerNewsStory())
                            {
                                throw new Exception($"Can't find comment {selectedCommentId}");
                            }

                            if (hackerNewsItem.kids != null)
                            {
                                storyCommentIds.AddRange(hackerNewsItem.kids);
                            }

                        }
                        else
                        {
                            Console.WriteLine("Internal server Error");
                        }


                        if (!existingComments.Any(x => x.CommentId == selectedCommentId))
                        {

                            var newComment = new Comment
                            {
                                CommentId = hackerNewsItem.id,
                                ParentCommentId = hackerNewsItem.parent,
                                SubmittedDate = hackerNewsItem.time,
                                CreatedBy = hackerNewsItem.by,
                                CreatedDate = now,
                                StoryId = storyId,
                            };

                            newComment.CommentText = new CommentText
                            {
                                CommentId = hackerNewsItem.id,
                                Text = hackerNewsItem.text
                            };

                            context.Comment.Add(newComment);
                            context.SaveChanges();
                            Console.WriteLine($"Adding Comment");
                        }
                        else if (existingComments.Any(x => x.CommentId == selectedCommentId && x.CommentText == null))
                        {
                            var existingComment = existingComments.FirstOrDefault(x => x.CommentId == selectedCommentId);


                            existingComment.SubmittedDate = hackerNewsItem.time;
                            existingComment.CreatedBy = hackerNewsItem.by;


                            existingComment.CommentText = new CommentText
                            {
                                Text = hackerNewsItem.text
                            };

                            context.Comment.Update(existingComment);
                            context.SaveChanges();
                            Console.WriteLine($"Adding Comment");
                        }
                        else
                        {
                            continue;
                        }
                    }

                    context.StoryProcessed.Add(new StoryProcessed
                    {
                        StoryId = storyId,
                        CreatedDate = now
                    });

                    context.SaveChanges();

                }


            }







        }

    }
}
