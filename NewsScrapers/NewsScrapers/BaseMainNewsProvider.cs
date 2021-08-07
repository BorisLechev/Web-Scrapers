using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewsScrapers
{
    public abstract class BaseMainNewsProvider
    {
        public abstract string BaseUrl { get; }

        public abstract Task<RemoteMainNews> GetMainNewsAsync();

        public async Task<IDocument> GetDocumentAsync(string url)
        {
            HtmlParser parser = new HtmlParser();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36");

            string html = await client.GetStringAsync(url);
            IHtmlDocument document = await parser.ParseDocumentAsync(html);

            return document;
        }
    }
}
