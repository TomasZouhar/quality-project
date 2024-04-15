namespace QualityProject.BL.Services;

public class DownloadService : IDownloadService
{
    public async Task<string> DownloadFileAsync()
    {
        using var httpClient = new HttpClient();
        const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.3";
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
        const string url = "https://ark-funds.com/wp-content/uploads/funds-etf-csv/ARK_INNOVATION_ETF_ARKK_HOLDINGS.csv";
        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}