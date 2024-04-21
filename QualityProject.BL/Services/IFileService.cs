namespace QualityProject.BL.Services;

public interface IFileService
{
    string GetFileFromDisk(string path);
    void SaveFileToDisk(string path, string content);
}