using HackerNewsScraper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerNewsScraper.Services
{
    public class ExtractBlogData
    {
        public void Execute()
        {
            using (var context = new HackerNewsContext())
            {

                var stories = context.Story.ToList();

                var blogs = context.Blog.ToList();
                var storyBlogs = context.StoryBlog.ToList();
                var blogArticles = context.BlogArticle.ToList();



                var blogStories = stories.Where(x => x.Url != null && ((x.Url.Contains("Blog", StringComparison.OrdinalIgnoreCase) || (x.Title != null && x.Title.Contains("Blog", StringComparison.OrdinalIgnoreCase))))).ToList();

                if (blogStories.Any())
                {
                    foreach (var story in blogStories)
                    {
                        var uri = new Uri(story.Url);
                        var baseUri = uri.GetLeftPart(UriPartial.Authority);

                        if (!blogs.Any(x => x.BaseUrl == baseUri))
                        {
                            context.Blog.Add(new Blog
                            {
                                BaseUrl = baseUri
                            });

                            context.SaveChanges();
                            Console.WriteLine($"Added blog: { baseUri }");
                        }

                        if (baseUri != story.Url)
                        {
                            var selectedBlog = blogs.FirstOrDefault(x => x.BaseUrl == baseUri);

                            if(selectedBlog == null)
                            {
                                continue;
                            }

                            if (!blogArticles.Any(x => x.Url == story.Url))
                            {
                                context.BlogArticle.Add(new BlogArticle
                                {
                                    Url = story.Url,
                                    BlogId = selectedBlog.BlogId
                                });

                                context.SaveChanges();
                                Console.WriteLine($"Added blog article: { story.Url }");
                            }
                        }

                    }

                    foreach (var story in blogStories)
                    {
                        var uri = new Uri(story.Url);
                        var baseUri = uri.GetLeftPart(UriPartial.Authority);
                        var blog = blogs.FirstOrDefault(x => x.BaseUrl == baseUri);

                        if (blog != null && !story.StoryBlog.Any(x => x.BlogId == blog.BlogId))
                        {
                            context.StoryBlog.Add(new StoryBlog
                            {
                                StoryId = story.StoryId,
                                BlogId = blog.BlogId
                            });

                            context.SaveChanges();
                            Console.WriteLine($"Added BlogArticle: { baseUri }");
                        }

                    }


                }




            }
        }
    }
}
