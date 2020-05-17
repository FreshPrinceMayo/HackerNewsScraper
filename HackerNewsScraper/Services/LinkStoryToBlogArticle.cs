using HackerNewsScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerNewsScraper.Services
{
    public class LinkStoryToBlogArticle
    {
        public void Execute()
        {
            using (var context = new HackerNewsContext())
            {
                var stories = context.Story.Where(x => !x.StoryBlogArticle.Any() && x.StoryBlog.Any()).ToList();
                var blogArticles = context.BlogArticle.ToList();


                foreach (var story in stories)
                {
                    var blogArticle = blogArticles.FirstOrDefault(x => x.Url == story.Url);

                    if(blogArticle != null)
                    {
                        context.StoryBlogArticle.Add(new StoryBlogArticle
                        {
                            BlogArticleId = blogArticle.BlogArticleId,
                            StoryId = story.StoryId
                        });

                        Console.WriteLine($"Added link to BlogArticle {blogArticle.BlogArticleId} to story {story.StoryId}");

                        context.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine($"Can't find blog article for {story.Url}");
                    }
                }


            }
        }
    }
}
