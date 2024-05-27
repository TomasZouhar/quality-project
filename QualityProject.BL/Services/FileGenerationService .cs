using PuppeteerSharp.Media;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QualityProject.DAL.Models;

namespace QualityProject.BL.Services
{
    public class FileGenerationService : IFileGenerationService
    {
        public async Task<FileResult> GenerateCsvFileAsync(string csvContent)
        {
            var content = Encoding.UTF8.GetBytes(csvContent);
            return await Task.FromResult(new FileResult
            {
                FileContent = content,
                ContentType = "text/csv",
                FileName = "stocks.csv",
                StringContent = csvContent
            });
        }

        public async Task<FileResult> GeneratePdfFileAsync(string htmlContent)
        {
            using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();

            await page.SetContentAsync(htmlContent);

            var pdfOptions = new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true
            };

            var pdfStream = await page.PdfStreamAsync(pdfOptions);
            using var memoryStream = new MemoryStream();
            await pdfStream.CopyToAsync(memoryStream);

            return new FileResult
            {
                FileContent = memoryStream.ToArray(),
                ContentType = "application/pdf",
                FileName = "stocks.pdf",
                StringContent = htmlContent
            };
        }

        public async Task<FileResult> GenerateHtmlFileAsync(string htmlContent)
        {
            var content = Encoding.UTF8.GetBytes(htmlContent);
            return await Task.FromResult(new FileResult
            {
                FileContent = content,
                ContentType = "text/html",
                FileName = "stocks.html",
                StringContent = htmlContent
            });
        }
    }
}
