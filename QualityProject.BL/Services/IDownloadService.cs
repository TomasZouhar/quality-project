namespace QualityProject.BL.Services;

public interface IDownloadService
{
    Task<string> DownloadFileAsync();
}