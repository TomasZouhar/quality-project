using QualityProject.DAL.Models;

namespace QualityProject.BL.Services;

public interface ICompareService
{
    /// <summary>
    /// Compares two files and returns the comparison result as a formatted string.
    /// </summary>
    /// <returns>The comparison result as a formatted string.</returns>
    Task<List<Holding>> CompareFilesStringAsync(string downloadedFileContent, string referenceFileContent);


}