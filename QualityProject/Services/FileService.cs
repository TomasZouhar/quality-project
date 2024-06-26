﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using QualityProject.Model;

namespace QualityProject.Services
{
    public class FileService : IFileService
    {
        private const string TestFileCsv = "date,fund,company,ticker,cusip,shares,\"market value ($)\",\"weight (%)\"\n03/14/2024,ARKK,\"COINBASE GLOBAL INC -CLASS A\",COIN,19260Q107,\"3,308,052\",\"$832,735,929.96\",10.66%\n03/14/2024,ARKK,\"BLOCK INC\",SQ,852234103,\"6,855,449\",\"$587,649,088.28\",7.52%\n03/14/2024,ARKK,\"TESLA INC\",TSLA,88160R101,\"3,392,047\",\"$574,884,125.56\",7.36%\n03/14/2024,ARKK,\"ROKU INC\",ROKU,77543R102,\"8,747,145\",\"$563,928,438.15\",7.22%\n03/14/2024,ARKK,\"UIPATH INC - CLASS A\",PATH,90364P105,\"19,583,336\",\"$478,420,898.48\",6.13%\n03/14/2024,ARKK,\"CRISPR THERAPEUTICS AG\",CRSP,H17182108,\"5,843,418\",\"$436,970,798.04\",5.60%\n03/14/2024,ARKK,\"ZOOM VIDEO COMMUNICATIONS-A\",ZM,98980L101,\"5,403,412\",\"$369,809,517.28\",4.74%\n03/14/2024,ARKK,\"ROBINHOOD MARKETS INC - A\",HOOD,770700102,\"19,428,376\",\"$333,390,932.16\",4.27%\n03/14/2024,ARKK,\"ROBLOX CORP -CLASS A\",RBLX,771049103,\"7,668,712\",\"$309,739,277.68\",3.97%\n03/14/2024,ARKK,\"UNITY SOFTWARE INC\",U,91332U101,\"9,193,564\",\"$240,687,505.52\",3.08%\n03/14/2024,ARKK,\"DRAFTKINGS INC-CL A\",\"DKNG UW\",26142V105,\"5,655,385\",\"$238,883,462.40\",3.06%\n03/14/2024,ARKK,\"PALANTIR TECHNOLOGIES INC-A\",PLTR,69608A108,\"9,100,951\",\"$227,523,775.00\",2.91%\n03/14/2024,ARKK,\"SHOPIFY INC - CLASS A\",SHOP,82509L107,\"2,841,408\",\"$223,561,981.44\",2.86%\n03/14/2024,ARKK,\"INTELLIA THERAPEUTICS INC\",NTLA,45826J105,\"7,474,957\",\"$214,307,017.19\",2.74%\n03/14/2024,ARKK,\"BEAM THERAPEUTICS INC\",BEAM,07373V105,\"5,667,679\",\"$211,801,164.23\",2.71%\n03/14/2024,ARKK,\"PAGERDUTY INC\",PD,69553P100,\"7,080,156\",\"$165,604,848.84\",2.12%\n03/14/2024,ARKK,\"TELADOC HEALTH INC\",TDOC,87918A105,\"9,831,262\",\"$148,845,306.68\",1.91%\n03/14/2024,ARKK,\"10X GENOMICS INC-CLASS A\",TXG,88025U109,\"3,851,977\",\"$148,378,154.04\",1.90%\n03/14/2024,ARKK,\"GINKGO BIOWORKS HOLDINGS INC\",DNA,37611X100,\"124,254,129\",\"$145,377,330.93\",1.86%\n03/14/2024,ARKK,\"RECURSION PHARMACEUTICALS-A\",RXRX,75629V104,\"13,022,915\",\"$142,991,606.70\",1.83%\n03/14/2024,ARKK,\"TRADE DESK INC/THE -CLASS A\",TTD,88339J105,\"1,535,616\",\"$123,094,978.56\",1.58%\n03/14/2024,ARKK,\"TWIST BIOSCIENCE CORP\",TWST,90184D100,\"3,389,060\",\"$120,447,192.40\",1.54%\n03/14/2024,ARKK,\"EXACT SCIENCES CORP\",EXAS,30063P105,\"1,846,115\",\"$112,760,704.20\",1.44%\n03/14/2024,ARKK,\"PINTEREST INC- CLASS A\",PINS,72352L106,\"2,977,007\",\"$103,004,442.20\",1.32%\n03/14/2024,ARKK,\"META PLATFORMS INC-CLASS A\",META,30303M102,\"188,092\",\"$93,212,752.44\",1.19%\n03/14/2024,ARKK,\"VERACYTE INC\",VCYT,92337F107,\"4,167,536\",\"$91,977,519.52\",1.18%\n03/14/2024,ARKK,\"ARCHER AVIATION INC-A\",ACHR,03945R102,\"17,453,767\",\"$89,712,362.38\",1.15%\n03/14/2024,ARKK,\"TWILIO INC - A\",TWLO,90138F102,\"1,366,653\",\"$85,142,481.90\",1.09%\n03/14/2024,ARKK,\"PACIFIC BIOSCIENCES OF CALIF\",PACB,69404D108,\"19,093,439\",\"$80,956,181.36\",1.04%\n03/14/2024,ARKK,\"TERADYNE INC\",TER,880770102,\"762,218\",\"$80,375,888.10\",1.03%\n03/14/2024,ARKK,\"SOFI TECHNOLOGIES INC\",SOFI,83406F102,\"9,949,424\",\"$73,426,749.12\",0.94%\n03/14/2024,ARKK,\"VERVE THERAPEUTICS INC\",VERV,92539P101,\"2,941,031\",\"$42,174,384.54\",0.54%\n03/14/2024,ARKK,\"MODERNA INC\",MRNA,60770K107,\"351,977\",\"$37,489,070.27\",0.48%\n03/14/2024,ARKK,\"CERUS CORP\",CERS,157085101,\"12,044,762\",\"$24,450,866.86\",0.31%\n03/14/2024,ARKK,\"PRIME MEDICINE INC\",PRME,74168J101,\"2,635,346\",\"$20,898,293.78\",0.27%\n03/14/2024,ARKK,\"NATERA INC\",NTRA,632307104,\"190,308\",\"$16,929,799.68\",0.22%\n03/14/2024,ARKK,\"GOLDMAN FS TRSY OBLIG INST 468\",,X9USDGSFT,\"16,306,448\",\"$16,306,447.99\",0.21%\n03/14/2024,ARKK,\"2U INC\",TWOU,90214J101,\"5,869,931\",\"$2,120,806.07\",0.03%\n\"Investors should carefully consider the investment objectives and risks as well as charges and expenses of an ARK ETF before investing. This and other information are contained in the ARK ETF's prospectuses, which may be obtained on ark-funds.com. The prospectus should be read carefully before investing. An investment in an ARK ETF is subject to risks and you can lose money on your investment in an ARK ETF. There can be no assurance that the ARK ETFs will achieve their investment objectives. The ARK ETFs' portfolios are more volatile than broad market averages. The ARK ETFs also have specific risks, which are described in the ARK ETFs' prospectuses, Shares of the ARK ETFs may be bought or sold throughout the day at their market price on the exchange on which they are listed. The market price of an ARK ETF's shares may be at, above or below the ARK ETF's net asset value (\"\"NAV\"\") and will fluctuate with changes in the NAV as well as supply and demand in the market for the shares. The market price of ARK ETF shares may differ significantly from their NAV during periods of market volatility. Shares of the ARK ETFs may only be redeemed directly with the ARK ETFs at NAV by Authorized Participants, in very large creation units. There can be no guarantee that an active trading market for ARK ETF shares will develop or be maintained, or that their listing will continue or remain unchanged. Buying or selling ARK ETF shares on an exchange may require the payment of brokerage commissions and frequent trading may incur brokerage costs that detract significantly from investment returns. Not FDIC Insured – No Bank Guarantee – May Lose Value. All statements made regarding companies, securities or other financial information are strictly beliefs and points of view held by ARK Investment Management LLC and/or ARK ETF Trust and are not endorsements by ARK of any company or security or recommendations by ARK to buy, sell or hold any security. Holdings are subject to change without notice. Foreside Fund Services, LLC, distributor. \u00a9 2024. ARK ETF Trust. No part of this material may be reproduced in any form, or referred to in any other publication, without written permission.\"\n";

        private static async Task<string> DownloadFileAsync()
        {
            using var httpClient = new HttpClient();
            var UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3";
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
            const string url = "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        private static Task<string> CompareFilesStringAsync(string fileContent1, string fileContent2)
        {
            var oldHoldings = ParseCsvString(fileContent1).OrderBy(h => h.Company).ToList();
            var newHoldings = ParseCsvString(fileContent2).OrderBy(h => h.Company).ToList();
            var subtractedHoldings = oldHoldings.Zip(newHoldings, (oldHolding, newHolding) => newHolding - oldHolding).ToList();
            subtractedHoldings = subtractedHoldings.OrderByDescending(h => h.WeightPercentage).ToList();
            var result = FormatHoldingsTable(subtractedHoldings);
            return Task.FromResult(result.ToString());
        }
        
        private static Task<string> CompareReducedFilesStringAsync(string fileContent1, string fileContent2)
        {
            var oldHoldings = ParseCsvString(fileContent1).OrderBy(h => h.Company).ToList();
            var newHoldings = ParseCsvString(fileContent2).OrderBy(h => h.Company).ToList();
            var subtractedHoldings = oldHoldings.Zip(newHoldings, (oldHolding, newHolding) => newHolding - oldHolding).ToList();
            subtractedHoldings = subtractedHoldings.OrderByDescending(h => h.WeightPercentage).ToList();
            var result = FormatReducedHoldingsTable(subtractedHoldings);
            return Task.FromResult(result.ToString());
        }

        private static StringBuilder FormatHoldingsTable(List<Holding> subtractedHoldings)
        {
            var result = new StringBuilder();
            result.Append($"{nameof(Holding.Date),-10} | {nameof(Holding.Fund),-35} | {nameof(Holding.Company),-40} | {nameof(Holding.Ticker),-10} | {nameof(Holding.Cusip),-10} | {nameof(Holding.Shares),10} | {nameof(Holding.MarketValueUsd),15} | {nameof(Holding.WeightPercentage),10}\n");
            foreach (var holding in subtractedHoldings)
            {
                result.AppendLine(holding.ToString());
            }

            return result;
        }
        
        private static StringBuilder FormatReducedHoldingsTable(List<Holding> subtractedHoldings)
        {
            var result = new StringBuilder();
            result.Append($"{nameof(Holding.Ticker)} | {nameof(Holding.Shares)}\n");
            foreach (var holding in subtractedHoldings)
            {
                result.AppendLine($"{holding.Ticker} | {holding.Shares}");
            }

            return result;
        }

        private static List<Holding> ParseCsvString(string csvData)
        {
            var holdings = new List<Holding>();

            using var reader = new StringReader(csvData);
            using var parser = new TextFieldParser(reader);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            parser.ReadLine();

            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();

                if (fields.Length < 8)
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

        public async Task<string> CompareFileAsync()
        {
            var downloadedFileContent = await DownloadFileAsync();
            var diffHtml = await CompareFilesStringAsync(TestFileCsv, downloadedFileContent);
            return diffHtml;
        }
        
        public async Task<string> CompareFileReducedAsync()
        {
            var downloadedFileContent = await DownloadFileAsync();
            var diffHtml = await CompareReducedFilesStringAsync(TestFileCsv, downloadedFileContent);
            return diffHtml;
        }
    }
}
