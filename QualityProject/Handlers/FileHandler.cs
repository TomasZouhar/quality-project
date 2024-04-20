using QualityProject.BL.Services;

namespace QualityProject.API.Handlers;

public static class FileHandler
{
    public static async Task<IResult> CompareFiles(ICompareService cs, IFileService fileService)
    {
        var result = await cs.CompareFileAsync(fileService.GetFileFromDisk("referenceFile.csv"));
        return Results.Content(result, "text/plain");
    }
}