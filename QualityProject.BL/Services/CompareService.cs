using System.Globalization;
using Microsoft.VisualBasic.FileIO;
using QualityProject.BL.Exceptions;
using QualityProject.DAL.Models;

namespace QualityProject.BL.Services
{
    public class CompareService : ICompareService
    {

        private string Path = "referenceFile.csv";

        public CompareService()
        {
        }

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

        public async Task<List<Holding>> CompareFilesStringAsync(string downloadedFileContent, string referenceFileContent)
        {
            try
            {
                var oldHoldings = ParseCsvString(referenceFileContent).OrderBy(h => h.Company).ToList();
                var newHoldings = ParseCsvString(downloadedFileContent).OrderBy(h => h.Company).ToList();
                var subtractedHoldings = oldHoldings.Zip(newHoldings, (oldHolding, newHolding) => new Holding
                {
                    Date = newHolding.Date,
                    Fund = newHolding.Fund,
                    Company = newHolding.Company,
                    Ticker = newHolding.Ticker,
                    Cusip = newHolding.Cusip,
                    Shares = newHolding.Shares - oldHolding.Shares,
                    MarketValueUsd = newHolding.MarketValueUsd - oldHolding.MarketValueUsd,
                    WeightPercentage = newHolding.WeightPercentage - oldHolding.WeightPercentage
                }).ToList();
                return subtractedHoldings = subtractedHoldings.OrderByDescending(h => h.WeightPercentage).ToList();
            }
            catch (Exception e)
            {
                throw new CustomException("An error occurred while comparing the files.", 500, e);
            }
        }
    }
}
