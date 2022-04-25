// See https://aka.ms/new-console-template for more information
using AngleSharp.Html.Parser;
using ImotBgScraper;
using System.Text;
using System.Text.RegularExpressions;

//var properties = new ImotBgDataGather().GatherDataAsync(1, 1000);
var properties = new List<Property>();

HtmlParser parser = new HtmlParser();
HttpClientHandler handler = new HttpClientHandler { AllowAutoRedirect = false, };
HttpClient httpClient = new HttpClient(handler);

for (int size = 10; size <= 1000; size++)
{
    Console.Write($"Area ${size}: ");

    // F12 -> Network tab -> imot.cgi -> Payload tab -> Form Data
    var formDataFlats = $"act=3&rub=1&rub_pub_save=1&topmenu=2&actions=1&f0=127.0.0.1&f1=1&f2=&f3=&f4=1&f7=1%7E2%7E3%7E4%7E5%7E6%7E8%7E&f28=&f29=&f43=&f44=&f30=EUR&f26={size}&f27={size}&f41=1&f31=&f32=&f38=%E3%F0%E0%E4+%D1%EE%F4%E8%FF&f42=&f39=&f40=&fe3=&fe4=&f45=&f46=&f51=&f52=&f33=&f34=&f35=&f36=&f37=&fe2=1";
    var formDataHouses = $"act=3&rub=1&rub_pub_save=1&topmenu=2&actions=1&f0=127.0.0.1&f1=1&f2=&f3=&f4=1&f7=10~&f28=&f29=&f43=&f44=&f30=EUR&f26={size}&f27={size}&f41=1&f31=&f32=&f38=&f42=&f39=&f40=&f3=&fe4=&f45=&f46=&f51=&f52=&f33=&f34=&f35=&f36=&f37=&fe2=1";

    var response = await httpClient.PostAsync(
                "https://www.imot.bg/pcgi/imot.cgi",
                new StringContent(formDataFlats, Encoding.UTF8, "application/x-www-form-urlencoded"));
    var firstPageUrl = response.Headers.Location;

    // pagination
    for (int page = 1; page <= 26; page++)
    {
        var pageUrl = firstPageUrl.ToString().Replace("&f1=1", $"&f1={page}");
        var adsPageHtml = await httpClient.GetStringAsync(pageUrl);
        var adsPageDocument = await parser.ParseDocumentAsync(adsPageHtml);
        var listItems = adsPageDocument
                        .QuerySelectorAll("a.photoLink")
                        .Where(x => x.Attributes["href"]?.Value?.Contains("//www.imot.bg/pcgi/imot.cgi?act=5&adv=") == true)
                        .ToList();

        if (listItems.Any() == false)
        {
            break;
        }

        foreach (var listItem in listItems)
        {
            var adUrl = "https:" + listItem.Attributes["href"].Value;

            var adDetailBytesArray = await httpClient.GetByteArrayAsync(adUrl);
            var adDetailsHtmlWithEncoding = Encoding.GetEncoding("windows-1251").GetString(adDetailBytesArray); // document.characterSet -> windows-1251

            var adDetailsDocument = await parser.ParseDocumentAsync(adDetailsHtmlWithEncoding);
            var district = adDetailsHtmlWithEncoding
                .GetStringBetween("<span style=\"font-size:14px; margin:8px 0; display:inline-block\">", "</span>")
                ?.Trim();

            // Input: град София, Банишора, ул. Сливен
            // Output: град София, Банишора
            if (district?.Contains("<br>") == true)
            {
                var indexOfBr = district.IndexOf("<br>", StringComparison.InvariantCulture);
                district = district.Substring(0, indexOfBr).Trim();
            }

            var floorsRegex = new Regex(@"^(?<floor>[0-9]+)[^\d]+(?<totalFloors>[0-9]+)$", RegexOptions.Compiled); // 12-ти от 18
            var buildingTypeAndYearRegex = new Regex(@"^(?<buildingType>[^,]+)([,\s]+(?<year>\d+))?", RegexOptions.Compiled); // Тухла, 2023 г.

            var floorInfoString = adDetailsHtmlWithEncoding
                .GetStringBetween("<li>Етаж:</li><li>", "</li>")
                .Replace("Партер", "1");
            var floorMatch = floorsRegex.Match(floorInfoString);
            var buildingTypeAndYearString = adDetailsHtmlWithEncoding
                .GetStringBetween("<li>Строителство:</li><li>", "</li>");
            var buildingTypeMatch = buildingTypeAndYearRegex.Match(buildingTypeAndYearString);
            var yardSizeString = adDetailsHtmlWithEncoding
                .GetStringBetween("<li>Двор:</li><li>", "</li>")
                .Replace(" кв.м", string.Empty)
                .Trim();
            int.TryParse(yardSizeString, out var yardSize);

            var property = new Property
            {
                Url = adUrl,
                Size = size,
                YardSize = yardSize,
                District = district,
                Floor = floorMatch.Success
                        ? floorMatch.Groups["floor"].Value.ToInteger()
                        : 0,
                TotalFloors = floorMatch.Success
                            ? floorMatch.Groups["totalFloors"].Value.ToInteger()
                            : 0,
                Price = adDetailsDocument
                        .QuerySelector("span#cena")?.TextContent
                        ?.Replace(" EUR", string.Empty)
                        ?.ToInteger()
                        ?? 0,
                BuildingType = buildingTypeMatch.Success
                        ? buildingTypeMatch.Groups["buildingType"].Value
                        : null,
                Year = buildingTypeMatch.Success && buildingTypeMatch.Groups["year"].Success
                        ? buildingTypeMatch.Groups["year"].Value.ToInteger()
                        : 0,
                Type = adDetailsHtmlWithEncoding
                        .GetStringBetween("<h1 style=\"margin: 0; font-size:18px;\">", "</h1>")
                        ?.Replace("Продава", string.Empty)
                        .Trim(),
            };

            properties.Add(property);
        }

        Console.Write($"{page}({listItems.Count}), ");
    }

    Console.WriteLine($" => Total: {properties.Count}");

    // 17341 records in imot.bg-2022-04-25.csv
    //File.WriteAllText(
    //   $"imot.bg-raw-data-{DateTime.Now:yyyy-MM-dd}.json",
    //   JsonConvert.SerializeObject(properties, Formatting.Indented));

    //using var csvWriter = new CsvWriter(new StreamWriter(File.OpenWrite($"imot.bg-raw-data-{DateTime.Now:yyyy-MM-dd}.csv"), Encoding.UTF8), CultureInfo.CurrentCulture);
    //csvWriter.WriteRecords(properties);
}
