namespace QualityProject.BL.Services;

public class FileService : IFileService
{
    public string GetFileFromDisk(string path)
    {
        return System.IO.File.ReadAllText(path);
    }
}