using QualityProject.BL.Services;

namespace QualityProject.API.Handlers;

public static class FileHandler
{
    /// <summary>
    /// Compares a file with a reference file
    /// </summary>
    /// <param name="cs">Compare Service</param>
    /// <param name="fileService">File Service</param>
    /// <returns>Comparison result</returns>
    public static async Task<IResult> CompareFiles(ICompareService cs, IFileService fileService)
    {
        var result = await cs.CompareFileAsync();
        return Results.Content(result, "text/plain");
    }
    
    /// <summary>
    /// Downloads a file and saves it to disk
    /// </summary>
    /// <param name="ds">Download Service</param>
    /// <param name="fileService">File Service</param>
    /// <returns>OK if content is not empty</returns>
    public static async Task<IResult> DownloadAndSaveReferenceFile(IDownloadService ds, IFileService fileService)
    {
        var content = await ds.DownloadFileAsync();
        if (string.IsNullOrEmpty(content))
        {
            return Results.BadRequest("Content is empty");
        }
        fileService.SaveFileToDisk("referenceFile.csv", content);
        return Results.Ok();
    }
}