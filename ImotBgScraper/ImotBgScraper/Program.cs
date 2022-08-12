// See https://aka.ms/new-console-template for more information
using CsvHelper;
using ImotBgScraper;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

var gather = new ImotBgDataGather();
var propertiesInSofia = await gather.GatherDataAsync(10, 1000, "%D1%EE%F4%E8%FF");
var propertiesInVarna = await gather.GatherDataAsync(10, 1000, "%C2%E0%F0%ED%E0");
var combinedPropertyCollections = new List<Property>(propertiesInSofia);
combinedPropertyCollections.AddRange(propertiesInVarna);

// 17341 records in imot.bg-raw-data-2022-04-25.csv
// 20200 records in imot.bg-sofia-raw-data-2022-08-12.csv
// 10227 records in imot.bg-varna-raw-data-2022-08-12.csv
//File.WriteAllText(
//   $"imot.bg-sofia-raw-data-{DateTime.Now:yyyy-MM-dd}.json",
//   JsonConvert.SerializeObject(propertiesInSofia, Formatting.Indented));

//File.WriteAllText(
//   $"imot.bg-varna-raw-data-{DateTime.Now:yyyy-MM-dd}.json",
//   JsonConvert.SerializeObject(propertiesInVarna, Formatting.Indented));

//File.WriteAllText(
//   $"imot.bg-combined-property-collections-raw-data-{DateTime.Now:yyyy-MM-dd}.json",
//   JsonConvert.SerializeObject(combinedPropertyCollections, Formatting.Indented));

//using var csvWriterSofia = new CsvWriter(new StreamWriter(File.OpenWrite($"imot.bg-sofia-raw-data-{DateTime.Now:yyyy-MM-dd}.csv"), Encoding.UTF8), CultureInfo.CurrentCulture);
//await csvWriterSofia.WriteRecordsAsync(propertiesInSofia);

//using var csvWriterVarna = new CsvWriter(new StreamWriter(File.OpenWrite($"imot.bg-varna-raw-data-{DateTime.Now:yyyy-MM-dd}.csv"), Encoding.UTF8), CultureInfo.CurrentCulture);
//await csvWriterVarna.WriteRecordsAsync(propertiesInVarna);

//using var csvWriterCombined = new CsvWriter(new StreamWriter(File.OpenWrite($"imot.bg-combined-property-collections-raw-data-{DateTime.Now:yyyy-MM-dd}.csv"), Encoding.UTF8), CultureInfo.CurrentCulture);
//await csvWriterCombined.WriteRecordsAsync(combinedPropertyCollections);

// 13657 records in imot.bg-2022-04-25.csv
// 15604 records in imot.bg-sofia-2022-08-12.csv
// 7338 records in imot.bg-varna-2022-08-12.csv
var filteredPropertiesInSofia = propertiesInSofia
                        .Where(p => p.Price > 0 && p.Year > 0 && p.Floor > 0 && p.TotalFloors > 0)
                        .ToList();
var filteredPropertiesInVarna = propertiesInVarna
                        .Where(p => p.Price > 0 && p.Year > 0 && p.Floor > 0 && p.TotalFloors > 0)
                        .ToList();
var filteredPropertiesCombined = combinedPropertyCollections
                        .Where(p => p.Price > 0 && p.Year > 0 && p.Floor > 0 && p.TotalFloors > 0)
                        .ToList();

//using var csvWriterFilteredSofia = new CsvWriter(new StreamWriter(File.OpenWrite($"imot.bg-sofia-{DateTime.Now:yyyy-MM-dd}.csv"), Encoding.UTF8), CultureInfo.CurrentCulture);
//await csvWriterFilteredSofia.WriteRecordsAsync(filteredPropertiesInSofia);

//using var csvWriterFilteredVarna = new CsvWriter(new StreamWriter(File.OpenWrite($"imot.bg-varna-{DateTime.Now:yyyy-MM-dd}.csv"), Encoding.UTF8), CultureInfo.CurrentCulture);
//await csvWriterFilteredVarna.WriteRecordsAsync(filteredPropertiesInVarna);

//using var csvWriterFilteredCombined = new CsvWriter(new StreamWriter(File.OpenWrite($"imot.bg-combined-{DateTime.Now:yyyy-MM-dd}.csv"), Encoding.UTF8), CultureInfo.CurrentCulture);
//await csvWriterFilteredCombined.WriteRecordsAsync(filteredPropertiesCombined);