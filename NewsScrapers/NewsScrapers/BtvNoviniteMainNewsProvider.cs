using System.Linq;
using System.Threading.Tasks;

namespace NewsScrapers
{
    public class BtvNoviniteMainNewsProvider : BaseMainNewsProvider
    {
        public override string BaseUrl => "https://btvnovinite.bg";

        public async override Task<RemoteMainNews> GetMainNewsAsync()
        {
            var mainDocument = await this.GetDocumentAsync(this.BaseUrl);

            var title = mainDocument.QuerySelector(".leading-articles .item .title").InnerHtml;

            var urlElement = mainDocument.QuerySelector(".leading-articles .item .link").Attributes["href"].Value.Trim();
            var articleUrl = this.BaseUrl + urlElement;

            var imageUrl = mainDocument.QuerySelector(".leading-articles .item .image img")?.Attributes["src"].Value.Trim();

            var articleDocument = await this.GetDocumentAsync(articleUrl);

            var articleAuthor = articleDocument.QuerySelector(".article-details .name a").InnerHtml;

            var articlePublishDate = articleDocument
                .QuerySelector(".article-details .date-wrapper .published")
                .InnerHtml
                .Trim();

            var articleContent = articleDocument
                .QuerySelectorAll(".article-body p")
                .Select(x => x.InnerHtml.Trim())
                .ToList();

            var articleImages = articleDocument
                .QuerySelectorAll(".article-body .image img")
                .Select(x => x.Attributes["src"].Value.Trim())
                .ToList();

            var articleTags = articleDocument
                .QuerySelectorAll(".keywords-wrapper ul li a")
                .Select(x => x.InnerHtml.Trim())
                .ToList();

            return new RemoteMainNews(title, articleUrl, imageUrl, articlePublishDate, articleAuthor, articleContent, articleImages, articleTags);
        }
    }
}
