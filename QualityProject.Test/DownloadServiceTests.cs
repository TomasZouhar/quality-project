using System.Net;
using Moq;
using Moq.Protected;
using QualityProject.BL.Services;

namespace QualityProject.Test
{
    public class DownloadServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly DownloadService _downloadService;

        public DownloadServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv")
            };
            _downloadService = new DownloadService(_httpClient);
        }

        private void SetupResponse(HttpStatusCode statusCode, string content)
        {
            // https://medium.com/younited-tech-blog/easy-httpclient-mocking-3395d0e5c4fa
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(content)
                });
        }

        [Fact]
        public async Task DownloadFileAsyncSuccess()
        {
            // Arrange
            const string expectedContent = """
                date,fund,company,ticker,cusip,shares,"market value ($)","weight (%)"
                03/14/2024,ARKK,"COINBASE GLOBAL INC -CLASS A",COIN,19260Q107,"3,308,052","$832,735,929.96",10.66%

                """;
            SetupResponse(HttpStatusCode.OK, expectedContent);

            // Act
            var result = await _downloadService.DownloadFileAsync();

            // Assert
            Assert.Equal(expectedContent, result);
        }

        [Fact]
        public void DownloadFileAsyncBadRequest()
        {
            // Arrange
            SetupResponse(HttpStatusCode.BadRequest, "Bad request");

            // Act  &  Assert
            Assert.ThrowsAsync<HttpRequestException>(() => _downloadService.DownloadFileAsync());
        }
    }
}