using System.Text;
using Moq;
using QualityProject.BL.Services;
using QualityProject.DAL.Models;

namespace QualityProject.Tests
{
    public class CompareServiceTests
    {
        private Mock<IDownloadService> downloadServiceMock;
        private Mock<IFormatService> formatServiceMock;
        private Mock<IFileService> fileServiceMock;
        private CompareService compareService;

        public CompareServiceTests()
        {
            downloadServiceMock = new Mock<IDownloadService>();
            formatServiceMock = new Mock<IFormatService>();
            fileServiceMock = new Mock<IFileService>();

            compareService = new CompareService(downloadServiceMock.Object, formatServiceMock.Object, fileServiceMock.Object);
        }
        
        
        [Fact]
        public async Task CompareFileAsync_ReturnsExpectedResult()
        {
            
           var downloadedFileContent = @"date,fund,company,ticker,cusip,shares,""market value ($)"",""weight (%)""\n" +
                 @"04/16/2024,ARKK,""TESLA INC"",TSLA,88160R101,""4,028,071"",""$650,452,905.08"",9.83%\n" +
                 @"04/16/2024,ARKK,""COINBASE GLOBAL INC -CLASS A"",COIN,19260Q107,""2,630,233"",""$587,620,354.53"",8.88%\n" +
                 @"""Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (""""NAV"""") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. © 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.";
           
           var referenceFile  = @"date,fund,company,ticker,cusip,shares,""market value ($)"",""weight (%)""\n" +
                 @"04/16/2024,ARKK,""TESLA INC"",TSLA,88160R101,""4,028,071"",""$650,452,905.08"",8.83%\n" +
                 @"04/16/2024,ARKK,""COINBASE GLOBAL INC -CLASS A"",COIN,19260Q107,""2,630,233"",""$587,620,354.53"",8.88%\n" +
                 @"""Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (""""NAV"""") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. © 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.";
            var expected = @"date,fund,company,ticker,cusip,shares,""market value ($)"",""weight (%)""\n" +
                 @"04/16/2024,ARKK,""TESLA INC"",TSLA,88160R101,""4,028,071"",""$650,452,905.08"",1.83%\n" +
                 @"04/16/2024,ARKK,""COINBASE GLOBAL INC -CLASS A"",COIN,19260Q107,""2,630,233"",""$587,620,354.53"",8.88%\n" +
                 @"""Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (""""NAV"""") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. © 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.";
            
            // Arrange
            downloadServiceMock.Setup(x => x.DownloadFileAsync()).ReturnsAsync(downloadedFileContent);
            fileServiceMock.Setup(x => x.GetFileFromDisk(It.IsAny<string>())).Returns(referenceFile);
            formatServiceMock.Setup(x => x.FormatHoldingsTable(It.IsAny<List<Holding>>())).Returns(new StringBuilder(expected));

            // Act
            var result = await compareService.CompareFileAsync();

            // Assert
            Assert.Equal(expected.ToString(), result);
        }

        [Fact]
        public async Task CompareFileReducedAsync_ReturnsExpectedResult()
        {
             var downloadedFileContent = @"date,fund,company,ticker,cusip,shares,""market value ($)"",""weight (%)""\n" +
                 @"04/16/2024,ARKK,""TESLA INC"",TSLA,88160R101,""4,028,071"",""$650,452,905.08"",9.83%\n" +
                 @"04/16/2024,ARKK,""COINBASE GLOBAL INC -CLASS A"",COIN,19260Q107,""2,630,233"",""$587,620,354.53"",8.88%\n" +
                 @"""Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (""""NAV"""") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. © 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.";
           
           var referenceFile  = @"date,fund,company,ticker,cusip,shares,""market value ($)"",""weight (%)""\n" +
                 @"04/16/2024,ARKK,""TESLA INC"",TSLA,88160R101,""4,028,071"",""$650,452,905.08"",8.83%\n" +
                 @"04/16/2024,ARKK,""COINBASE GLOBAL INC -CLASS A"",COIN,19260Q107,""2,630,233"",""$587,620,354.53"",8.88%\n" +
                 @"""Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (""""NAV"""") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. © 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.";
            var expected = @"date,fund,company,ticker,cusip,shares,""market value ($)"",""weight (%)""\n" +
                 @"04/16/2024,ARKK,""TESLA INC"",TSLA,88160R101,""4,028,071"",""$650,452,905.08"",1.83%\n" +
                 @"04/16/2024,ARKK,""COINBASE GLOBAL INC -CLASS A"",COIN,19260Q107,""2,630,233"",""$587,620,354.53"",8.88%\n" +
                 @"""Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (""""NAV"""") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. © 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.";

            
            // Arrange
            downloadServiceMock.Setup(x => x.DownloadFileAsync()).ReturnsAsync(downloadedFileContent);
            fileServiceMock.Setup(x => x.GetFileFromDisk(It.IsAny<string>())).Returns(referenceFile);
            formatServiceMock.Setup(x => x.FormatReducedHoldingsTable(It.IsAny<List<Holding>>())).Returns(new StringBuilder(expected));

            // Act
            var result = await compareService.CompareFileReducedAsync();

            // Assert
            Assert.Equal(expected, result.ToString());
        }

        [Fact]
        public async Task CompareFileHtmlAsync_ReturnsExpectedResult()
        {
             var downloadedFileContent = @"date,fund,company,ticker,cusip,shares,""market value ($)"",""weight (%)""\n" +
                 @"04/16/2024,ARKK,""TESLA INC"",TSLA,88160R101,""4,028,071"",""$650,452,905.08"",9.83%\n" +
                 @"04/16/2024,ARKK,""COINBASE GLOBAL INC -CLASS A"",COIN,19260Q107,""2,630,233"",""$587,620,354.53"",8.88%\n" +
                 @"""Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (""""NAV"""") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. © 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.";
           
           var referenceFile  = @"date,fund,company,ticker,cusip,shares,""market value ($)"",""weight (%)""\n" +
                 @"04/16/2024,ARKK,""TESLA INC"",TSLA,88160R101,""4,028,071"",""$650,452,905.08"",8.83%\n" +
                 @"04/16/2024,ARKK,""COINBASE GLOBAL INC -CLASS A"",COIN,19260Q107,""2,630,233"",""$587,620,354.53"",8.88%\n" +
                 @"""Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (""""NAV"""") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. © 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.";
            var expected = @"date,fund,company,ticker,cusip,shares,""market value ($)"",""weight (%)""\n" +
                 @"04/16/2024,ARKK,""TESLA INC"",TSLA,88160R101,""4,028,071"",""$650,452,905.08"",1.83%\n" +
                 @"04/16/2024,ARKK,""COINBASE GLOBAL INC -CLASS A"",COIN,19260Q107,""2,630,233"",""$587,620,354.53"",8.88%\n" +
                 @"""Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (""""NAV"""") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. © 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.";

            
            // Arrange
            downloadServiceMock.Setup(x => x.DownloadFileAsync()).ReturnsAsync(downloadedFileContent);
            fileServiceMock.Setup(x => x.GetFileFromDisk(It.IsAny<string>())).Returns(referenceFile);
            formatServiceMock.Setup(x => x.FormatHTMLHoldingsTable(It.IsAny<List<Holding>>())).Returns(expected);

            var compareService = new CompareService(downloadServiceMock.Object, formatServiceMock.Object, fileServiceMock.Object);

            // Act
            var result = await compareService.CompareFileHtmlAsync();

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
