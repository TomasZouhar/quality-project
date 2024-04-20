namespace QualityProject.BL.Services;

public interface ICompareService
{
    Task<string> CompareFileAsync();
    
    Task<string> CompareFileReducedAsync();
    
    Task<string> CompareFileHtmlAsync();
}