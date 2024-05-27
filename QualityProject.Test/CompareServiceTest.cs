using System.Text;
using Moq;
using QualityProject.BL.Services;
using QualityProject.DAL.Models;

namespace QualityProject.Test
{
    public class CompareServiceTests
    {
        private CompareService compareService;

        public CompareServiceTests()
        {
            compareService = new CompareService();
        }
        
        
        [Fact]
        public async Task CompareFileAsync_ReturnsExpectedResult()
        {
            // Test data
            var downloadedFileContent = "date,fund,company,ticker,cusip,shares,market value ($),weight (%)\n" +
                                        "04/16/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"4,028,071\",\"$650,452,905\",8%\n" +
                                        "04/16/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,630,233\",\"$587,620,354\",8%\n";

            var referenceFileContent = "date,fund,company,ticker,cusip,shares,market value ($),weight (%)\n" +
                                       "04/16/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"4,028,071\",\"$650,452,905\",9%\n" +
                                       "04/16/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"2,630,233\",\"$587,620,354\",8%\n";

            // Expected result
            var expected = new List<Holding>
            {
                new Holding
                {
                    Fund = "ARKK",
                    Ticker = "COIN",
                    Company = "COINBASE GLOBAL INC -CLASS A",
                    Cusip = "19260Q107",
                    Shares = 0,
                    MarketValueUsd = 0,
                    WeightPercentage = 0,
                    Date = new DateTime(2024, 4, 16)
                },
                new Holding
                {
                    Fund = "ARKK",
                    Ticker = "TSLA",
                    Company = "TESLA INC",
                    Cusip = "88160R101",
                    Shares = 0,
                    MarketValueUsd = 0,
                    WeightPercentage = -1,
                    Date = new DateTime(2024, 4, 16)
                }
            };

            // Act
            var result = await compareService.CompareFilesStringAsync(downloadedFileContent, referenceFileContent);

            // Assert
            Assert.Equal(expected, result);

        }
        
        [Fact]
        public async Task Holdings_operator_test()
        {
            var holdings = new List<Holding>();
            holdings.Add(new Holding
            {
                Fund = "Test",
                Ticker = "TST",
                Company = "Test Company",
                Cusip = "123456789",
                Shares = 100,
                MarketValueUsd = 1000,
                WeightPercentage = 10,
                Date = new DateTime(2024, 4, 16)
            });
            holdings.Add(new Holding
            {
                Fund = "Test",
                Ticker = "TST",
                Company = "Test Company",
                Cusip = "123456789",
                Shares = 75,
                MarketValueUsd = 1000,
                WeightPercentage = 10,
                Date = new DateTime(2024, 4, 16)
            });
            holdings.Add(new Holding
            {
                Fund = "Test",
                Ticker = "TST",
                Company = "Test Company",
                Cusip = "123456789",
                Shares = 25,
                MarketValueUsd = 0,
                WeightPercentage = 0,
                Date = new DateTime(2024, 4, 16)
            });

            
            
            
            
            // Act
            var result = holdings[0] - holdings[1];

            // Assert
            Assert.Equal(holdings[2].Date, result.Date);
            Assert.Equal(holdings[2].Fund, result.Fund);
            Assert.Equal(holdings[2].Company, result.Company);
            Assert.Equal(holdings[2].Ticker, result.Ticker);
            Assert.Equal(holdings[2].Cusip, result.Cusip);
            Assert.Equal(holdings[2].Shares, result.Shares);
            Assert.Equal(holdings[2].MarketValueUsd, result.MarketValueUsd);
            Assert.Equal(holdings[2].WeightPercentage, result.WeightPercentage);
        }
    }
}
