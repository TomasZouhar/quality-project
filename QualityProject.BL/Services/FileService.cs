using QualityProject.BL.Exceptions;

namespace QualityProject.BL.Services;

public class FileService : IFileService
{
    public string GetFileFromDisk(string path)
    {
        try
        {
            return File.ReadAllText(path);
        }
        catch (Exception e)
        {
            throw new CustomException("An error occurred while reading the file.", 500, e);
        }
        
    }
    
    public void SaveFileToDisk(string path, string content)
    {
        try
        {
            File.WriteAllText(path, content);
        }
        catch (Exception e)
        {
            throw new CustomException("An error occurred while saving the file.", 500, e);
        }
    }
}