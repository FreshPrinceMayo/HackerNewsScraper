using HackerNewsScraper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackerNewsScraper.Services
{
    public class ProcessCommentsBlogs
    {
        public void Excute()
        {
            using (var context = new HackerNewsContext())
            {
                var blogs  = context.Blog.Include(x => x.BlogArticle);
                var blogUrls = blogs.Select(x => x.BaseUrl).ToList();
                var blogArticleUrls = blogs.SelectMany(x => x.BlogArticle.Select(y => y.Url)).ToList();


                var commentUrls = context.CommentUrl.ToList();

                foreach(var commentUrl in commentUrls)
                {
                    if(blogArticleUrls.Contains(commentUrl.Url))
                    {
                        continue;
                    }


                    var uri = new Uri(commentUrl.Url);
                    var baseUri = uri.GetLeftPart(UriPartial.Authority);

                    if (blogUrls.Contains(baseUri.ToString()))
                    {
                        var blog = blogs.FirstOrDefault(x => x.BaseUrl == baseUri.ToString());

                        if(blog.BaseUrl != uri.ToString())
                        {
                            context.BlogArticle.Add(new BlogArticle
                            {
                                BlogId = blog.BlogId,
                                Url = uri.ToString()
                            });

                            context.SaveChanges();
                            Console.WriteLine($"Adding blog article {uri.ToString()}");
                        }
                    }

                }

                var commentblogs = context.CommentBlog.ToList();

                foreach (var commentUrl in commentUrls)
                {
                    var uri = new Uri(commentUrl.Url);
                    var baseUri = uri.GetLeftPart(UriPartial.Authority);
                    var blog = blogs.FirstOrDefault(x => x.BaseUrl == baseUri.ToString());

                    if(blog != null && !commentblogs.Any(x => x.BlogId == blog.BlogId && x.CommentId == commentUrl.CommentId))
                    {
                        context.CommentBlog.Add(new CommentBlog
                        {
                            BlogId = blog.BlogId,
                            CommentId = commentUrl.CommentId
                        });

                        context.SaveChanges();
                    }
                }

            }
        }
    }
}
