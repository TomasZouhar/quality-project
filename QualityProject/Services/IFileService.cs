namespace QualityProject.Services;

public interface IFileService
{ 
    Task<string> CompareFileAsync();

}