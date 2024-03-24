namespace QualityProject.BL.Services;

public interface IFileService
{ 
    Task<string> CompareFileAsync();
    
    Task<string> CompareFileReducedAsync();

}