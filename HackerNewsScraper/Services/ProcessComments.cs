using HackerNewsScraper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace HackerNewsScraper.Services
{
    public class ProcessComments
    {
        public void Execute()
        {
            using (var context = new HackerNewsContext())
            {
                var comments = context.Comment.Include(x => x.CommentText).Where(x => x.HasLink == null).OrderByDescending(x => x.CommentId).ToList();

                foreach (var comment in comments)
                {
                    var commentText = comment.CommentText?.Text;

                    if (commentText != null && (commentText.Contains("href", StringComparison.InvariantCultureIgnoreCase) || commentText.Contains("www.",StringComparison.InvariantCultureIgnoreCase)))
                    {
                        var links = new List<string>();
                        var commentUrls = new List<CommentUrl>();
               

                        var decoded = HttpUtility.HtmlDecode(commentText);


                        MatchCollection matches = Regex.Matches(decoded, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

                        foreach (Match match in matches)
                        {
                            var cleanedMatch = match.Value.Replace("&#x2f", "/").Replace("&#x2F", "/");

                            if (CheckURLValid(cleanedMatch))
                            {
                                var validurl = new UriBuilder(cleanedMatch){Scheme = Uri.UriSchemeHttps, Port = -1 }.Uri.ToString();

                                commentUrls.Add(new CommentUrl
                                {
                                    CommentId = comment.CommentId,
                                    Url = validurl
                                });
                            }
                        }

                        if(commentUrls.Any())
                        {
                            var distinctUrls = commentUrls.GroupBy(x => x.Url).Select(x => x.First()).ToList();

                            Console.WriteLine($"Extracted {distinctUrls.Count()} links");
                            context.CommentUrl.AddRange(distinctUrls);
                            comment.HasLink = true;
                            context.Comment.Update(comment);
                            context.SaveChanges();
                        }
                        else
                        {
                            comment.HasLink = false;
                            context.Comment.Update(comment);
                            context.SaveChanges();
                        }

                       


                    }
                    else
                    {
                        comment.HasLink = false;
                        context.Comment.Update(comment);
                        context.SaveChanges();
                    }
                }


            }
        }

        public bool CheckURLValid(string source)
        {

            if(source.EndsWith(".."))
            {
                return false;
            }


            Uri uriResult;
            return Uri.TryCreate(source, UriKind.RelativeOrAbsolute, out uriResult);
        }

    }


}
