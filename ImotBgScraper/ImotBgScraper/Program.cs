// See https://aka.ms/new-console-template for more information
using CsvHelper;
using ImotBgScraper;
using Newtonsoft.Json;
using System.Globalization;
using System.Text;

var gather = new ImotBgDataGather();
var properties = await gather.GatherDataAsync(10, 1000);

// 17341 records in imot.bg-raw-data-2022-04-25.csv
//File.WriteAllText(
//   $"imot.bg-raw-data-{DateTime.Now:yyyy-MM-dd}.json",
//   JsonConvert.SerializeObject(properties, Formatting.Indented));

//using var csvWriter = new CsvWriter(new StreamWriter(File.OpenWrite($"imot.bg-raw-data-{DateTime.Now:yyyy-MM-dd}.csv"), Encoding.UTF8), CultureInfo.CurrentCulture);
//csvWriter.WriteRecords(properties);
