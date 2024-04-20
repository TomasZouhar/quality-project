namespace QualityProject.BL.Services;

public interface ICompareService
{
    /// <summary>
    /// Compares two files and returns the comparison result as a formatted string.
    /// </summary>
    /// <returns>The comparison result as a formatted string.</returns>
    Task<string> CompareFileAsync();

    /// <summary>
    /// Compares two files and returns a reduced comparison result as a formatted string.
    /// </summary>
    /// <returns>The reduced comparison result as a formatted string.</returns>
    Task<string> CompareFileReducedAsync();

    /// <summary>
    /// Compares two files and returns the comparison result as an HTML formatted string.
    /// </summary>
    /// <returns>The comparison result as an HTML formatted string.</returns>
    Task<string> CompareFileHtmlAsync();
}