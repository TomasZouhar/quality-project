using System.Globalization;
using Microsoft.VisualBasic.FileIO;
using QualityProject.DAL.Models;

namespace QualityProject.BL.Services;

public class CompareService : ICompareService
{ 
    private static IFormatService _formatService = new FormatService();
    private static IDownloadService _downloadService = new DownloadService();
    
    
            private static List<Holding> ParseCsvString(string csvData)
        {
            var holdings = new List<Holding>();

            using var reader = new StringReader(csvData);
            using var parser = new TextFieldParser(reader)
            {
                TextFieldType = FieldType.Delimited
            };
            parser.SetDelimiters(",");

            parser.ReadLine();

            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();

                if (fields?.Length < 8)
                    continue;

                var holding = new Holding
                {
                    Date = DateTime.ParseExact(fields[0], "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    Fund = fields[1],
                    Company = fields[2],
                    Ticker = fields[3],
                    Cusip = fields[4],
                    Shares = int.Parse(fields[5].Replace(",", "")),
                    MarketValueUsd = decimal.Parse(fields[6].Replace("$", "").Replace(",", "")),
                    WeightPercentage = decimal.Parse(fields[7].Replace("%", ""))
                };

                holdings.Add(holding);
            }

            return holdings;
        }

        private async Task<T> CompareFilesAsync<T>(string fileContent1, string fileContent2, Func<List<Holding>, T> formatMethod)
        {
            var oldHoldings = ParseCsvString(fileContent1).OrderBy(h => h.Company).ToList();
            var newHoldings = ParseCsvString(fileContent2).OrderBy(h => h.Company).ToList();
            var subtractedHoldings = oldHoldings.Zip(newHoldings, (oldHolding, newHolding) => newHolding - oldHolding).ToList();
            subtractedHoldings = subtractedHoldings.OrderByDescending(h => h.WeightPercentage).ToList();
            var result = formatMethod(subtractedHoldings);
            return await Task.FromResult(result);
        }


        public async Task<string> CompareFileAsync(string referenceFileContent)
        {
            var downloadedFileContent = await _downloadService.DownloadFileAsync();
            var result =
                await CompareFilesAsync(referenceFileContent, downloadedFileContent, _formatService.FormatHoldingsTable);
            return result.ToString();
        }

        public async Task<string> CompareFileReducedAsync(string referenceFileContent)
        {
            var downloadedFileContent = await _downloadService.DownloadFileAsync();
            var result = await CompareFilesAsync(referenceFileContent, downloadedFileContent, _formatService.FormatReducedHoldingsTable);
            return result.ToString();
        }
        
        public async Task<string> CompareFileHtmlAsync(string referenceFileContent)
        {
            var downloadedFileContent = await _downloadService.DownloadFileAsync();
            var result = await CompareFilesAsync(referenceFileContent, downloadedFileContent, _formatService.FormatHTMLHoldingsTable);
            return result;

        }
    }

