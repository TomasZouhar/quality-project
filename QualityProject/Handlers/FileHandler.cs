using QualityProject.BL.Services;

namespace QualityProject.API.Handlers;

public static class FileHandler
{
    public static async Task<IResult> CompareFiles(IFileService fileService)
    {
        var result = await fileService.CompareFileAsync();
        return Results.Content(result, "text/plain");
    }
}