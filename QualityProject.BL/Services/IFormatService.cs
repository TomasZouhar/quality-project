using System.Text;
using QualityProject.DAL.Models;

namespace QualityProject.BL.Services;

public interface IFormatService
{ 
    string FormatHTMLHoldingsTable(List<Holding> subtractedHoldings);
    StringBuilder FormatReducedHoldingsTable(List<Holding> subtractedHoldings);
    StringBuilder FormatHoldingsTable(List<Holding> subtractedHoldings);
}