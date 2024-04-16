namespace QualityProject.BL.Services;

public class FileService : IFileService
{
    public string GetFileFromDisk(string path)
    {
        return File.ReadAllText(path);
    }
}