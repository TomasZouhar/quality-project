namespace QualityProject.BL.Services;

public interface IDownloadService
{
    /// <summary>
    /// Asynchronously downloads a file and returns its content as a string
    /// </summary>
    /// <returns> This task returns the file content in its string representation</returns>
    Task<string> DownloadFileAsync();
}