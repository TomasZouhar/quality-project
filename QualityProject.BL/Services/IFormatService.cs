using System.Text;
using QualityProject.DAL.Models;

namespace QualityProject.BL.Services;

public interface IFormatService
{ 
    /// <summary>
    /// Formats holdings list into HTML table
    /// </summary>
    /// <param name="subtractedHoldings"></param>
    /// <returns>String containing HTML table of holdings</returns>
    string FormatHTMLHoldingsTable(List<Holding> subtractedHoldings);
    
    /// <summary>
    /// Format holdings list into reduced table
    /// </summary>
    /// <param name="subtractedHoldings"></param>
    /// <returns>StringBuilder containing reduced table of holding changes</returns>
    StringBuilder FormatReducedHoldingsTable(List<Holding> subtractedHoldings);
    
    /// <summary>
    /// Format holdings list into detailed table
    /// </summary>
    /// <param name="subtractedHoldings"></param>
    /// <returns>StringBuilder containing detailed table of holding changes</returns>
    StringBuilder FormatHoldingsTable(List<Holding> subtractedHoldings);
}