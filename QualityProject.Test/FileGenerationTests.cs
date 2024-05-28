using System.Text;
using QualityProject.BL.Services;

namespace QualityProject.Test
{
    public class FileGenerationTests
    {
        [Fact]
        public async Task GenerateCsvFileAsync_ReturnsCorrectFileResult()
        {
            // Arrange
            var service = new FileGenerationService();
            var csvContent = "header1,header2\nvalue1,value2";
    
            // Act
            var result = await service.GenerateCsvFileAsync(csvContent);
    
            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/csv", result.ContentType);
            Assert.Equal("stocks.csv", result.FileName);
            Assert.Equal(csvContent, result.StringContent);
            Assert.Equal(Encoding.UTF8.GetBytes(csvContent), result.FileContent);
        }
    
        [Fact]
        public async Task GeneratePdfFileAsync_ReturnsCorrectFileResult()
        {
            // Arrange
            var service = new FileGenerationService();
            var htmlContent = "<html><body><h1>Hello, World!</h1></body></html>";
    
            // Act
            var result = await service.GeneratePdfFileAsync(htmlContent);
    
            // Assert
            Assert.NotNull(result);
            Assert.Equal("application/pdf", result.ContentType);
            Assert.Equal("stocks.pdf", result.FileName);
            Assert.Equal(htmlContent, result.StringContent);
            Assert.NotEmpty(result.FileContent);
        }
    
        [Fact]
        public async Task GenerateHtmlFileAsync_ReturnsCorrectFileResult()
        {
            // Arrange
            var service = new FileGenerationService();
            var htmlContent = "<html><body><h1>Hello, World!</h1></body></html>";
    
            // Act
            var result = await service.GenerateHtmlFileAsync(htmlContent);
    
            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/html", result.ContentType);
            Assert.Equal("stocks.html", result.FileName);
            Assert.Equal(htmlContent, result.StringContent);
            Assert.Equal(Encoding.UTF8.GetBytes(htmlContent), result.FileContent);
        }
    }
}

