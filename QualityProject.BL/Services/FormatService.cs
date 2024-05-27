using System.Text;
using QualityProject.DAL.Models;

namespace QualityProject.BL.Services;

public class FormatService : IFormatService
{
    public string FormatHoldingsTable(List<Holding> subtractedHoldings)
    {
        var result = new StringBuilder();
        result.Append($"{nameof(Holding.Date),-10} | {nameof(Holding.Fund),-35} | {nameof(Holding.Company),-40} | {nameof(Holding.Ticker),-10} | {nameof(Holding.Cusip),-10} | {nameof(Holding.Shares),10} | {nameof(Holding.MarketValueUsd),15} | {nameof(Holding.WeightPercentage),10}\n");
        foreach (var holding in subtractedHoldings)
        {
            result.AppendLine(holding.ToString());
        }

        return result.ToString();
    }
    
    public string FormatReducedHoldingsTable(List<Holding> subtractedHoldings)
    {
        var result = new StringBuilder();
        result.Append($"{nameof(Holding.Ticker)} | {nameof(Holding.Shares)}\n");
        foreach (var holding in subtractedHoldings)
        {
            result.AppendLine($"{holding.Ticker} | {holding.Shares}");
        }

        return result.ToString();
    }
    
    public string FormatHTMLHoldingsTable(List<Holding> subtractedHoldings)
    {
        var result = new StringBuilder();
        result.Append("<table>");
        result.Append("<thead>");
        result.Append("<tr>");
        result.Append("<th>Ticker</th>");
        result.Append("<th>Shares</th>");
        result.Append("</tr>");
        result.Append("</thead>");
        result.Append("<tbody>");

        foreach (var holding in subtractedHoldings)
        {
            result.Append("<tr>");
            result.Append($"<td>{holding.Ticker}</td>");
            result.Append($"<td>{holding.Shares}</td>");
            result.Append("</tr>");
        }

        result.Append("</tbody>");
        result.Append("</table>");

        return result.ToString();
    }
}