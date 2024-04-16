namespace QualityProject.BL.Services;

public interface ICompareService
{
    Task<string> CompareFileAsync(string referenceFileContent);
    
    Task<string> CompareFileReducedAsync(string referenceFileContent);
    
    Task<string> CompareFileHtmlAsync(string referenceFileContent);
}