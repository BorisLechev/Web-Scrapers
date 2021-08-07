using System;
using System.Text;

namespace NewsScrapers
{
    class Program
    {
        static void Main(string[] args)
        {
            var btvNovinite = new BtvNoviniteMainNewsProvider();

            var result = btvNovinite.GetMainNewsAsync().GetAwaiter().GetResult();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"## Original Url: {result.OriginalUrl}");
            sb.AppendLine($"## Title: {result.Title}");
            sb.AppendLine($"## Image Url: {result.ImageUrl}");
            sb.AppendLine($"## Author: {result.ArticleAuthor}");
            sb.AppendLine($"## Publish Date: {result.ArticlePublishDate}");
            sb.AppendLine($"## Content: {string.Join(Environment.NewLine, result.ArticleContent)}");
            sb.AppendLine($"## Images: {string.Join(", ", result.ArticleImages)}");
            sb.AppendLine($"## Tags: {string.Join(", ", result.ArticleTags)}");

            Console.WriteLine(sb.ToString().TrimEnd());
        }
    }
}
