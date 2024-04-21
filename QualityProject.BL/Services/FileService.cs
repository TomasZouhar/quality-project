namespace QualityProject.BL.Services;

public class FileService : IFileService
{
    public string GetFileFromDisk(string path)
    {
        return File.ReadAllText(path);
    }
    
    public void SaveFileToDisk(string path, string content)
    {
        File.WriteAllText(path, content);
    }
}