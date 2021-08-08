using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BazarBgTownsNeighbourhoodsScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var baseUrl = "https://bazar.bg/obiavi/prodazhba-imoti";
            var dictionary = new Dictionary<string, string>();

            HtmlParser parser = new HtmlParser();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.131 Safari/537.36");

            string html = await client.GetStringAsync(baseUrl);
            IHtmlDocument document = await parser.ParseDocumentAsync(html);

            var towns = document
                .QuerySelectorAll("#autocompleteLocations a")
                .Select(x => x.InnerHtml.Trim(new[] { 'n', 'b', 's', 'p', ';', 'г', 'р', '.', ' ', '&' }))
                .ToList();

            var index = 0;

            foreach (var town in towns)
            {
                index++;

                if (index % 2 == 0)
                {
                    continue;
                }

                if (dictionary.ContainsKey(town) == false && string.IsNullOrEmpty(town) == false)
                {
                    var townWithLatinLetters = ConvertCyrilicToLatinLetters(town);

                    Console.WriteLine($"Town: {townWithLatinLetters}");

                    var neighbourhoodUrl = baseUrl + $"/{townWithLatinLetters}";

                    string htmlNeighbourhood = await client.GetStringAsync(neighbourhoodUrl);
                    IHtmlDocument neighbourhoodDocument = await parser.ParseDocumentAsync(htmlNeighbourhood);

                    var townNeighbourhoods = neighbourhoodDocument
                        .QuerySelectorAll("#locationGrid #regionsList .item-name")
                        .Select(x => x.InnerHtml.Trim())
                        .ToList();

                    foreach (var neighbourhood in townNeighbourhoods)
                    {
                        Console.WriteLine($"### Neighbourhood: {neighbourhood}");
                    }

                    dictionary[townWithLatinLetters] = townWithLatinLetters;
                    Console.WriteLine("------------------------------------------");
                }
            }
        }

        private static string ConvertCyrilicToLatinLetters(string town)
        {
            town = town.ToLower();

            var bulgarianLetters = new[]
            {
                "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п",
                "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ь", "ю", "я",
            };

            var latinRepresentationsOfBulgarianLetters = new[]
            {
                "a", "b", "v", "g", "d", "e", "zh", "z", "i", "y", "k",
                "l", "m", "n", "o", "p", "r", "s", "t", "u", "f", "h",
                "ts", "ch", "sh", "sht", "a", "i", "yu", "a",
            };

            for (int i = 0; i < bulgarianLetters.Length; i++)
            {
                town = town.Replace(bulgarianLetters[i], latinRepresentationsOfBulgarianLetters[i]);
            }

            return town.Replace(' ', '-');
        }
    }
}
