using System.Collections.Generic;

namespace NewsScrapers
{
    public class RemoteMainNews
    {
        public RemoteMainNews(string title, string originalUrl, string imageUrl, string articlePublishDate, string articleAuthor, IEnumerable<string> articleContent, IEnumerable<string> articleImages, IEnumerable<string> articleTags)
        {
            this.Title = title;
            this.OriginalUrl = originalUrl;
            this.ImageUrl = imageUrl;
            this.ArticlePublishDate = articlePublishDate;
            this.ArticleAuthor = articleAuthor;
            this.ArticleContent = articleContent;
            this.ArticleImages = articleImages;
            this.ArticleTags = articleTags;
        }

        public string Title { get; set; }

        public string OriginalUrl { get; set; }

        public string ImageUrl { get; set; }

        public string ArticlePublishDate { get; set; }

        public string ArticleAuthor { get; set; }

        public IEnumerable<string> ArticleContent { get; set; }

        public IEnumerable<string> ArticleImages { get; set; }

        public IEnumerable<string> ArticleTags { get; set; }
    }
}
