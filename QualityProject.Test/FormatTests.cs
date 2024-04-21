using QualityProject.BL.Services;
using QualityProject.DAL.Models;

namespace QualityProject.Test;

public class FormatTests
{
    private readonly FormatService _formatService = new FormatService();
    private const string EmptyTable = "<table><thead><tr><th>Ticker</th><th>Shares</th></tr></thead><tbody></tbody></table>";
    private const string HoldingTableHeader = "Date       | Fund                                | Company                                  | Ticker     | Cusip      |     Shares |  MarketValueUsd | WeightPercentage\n";
    private const string ReducedHoldingTableHeader = "Ticker | Shares\n";

    [Fact]
    public void HtmlEmptyStringTest()
    {
        // Arrange
        var holdings = new List<Holding>();
        
        // Act
        var output = _formatService.FormatHTMLHoldingsTable(holdings);
        
        // Assert
        Assert.Equal(EmptyTable, output);
    }
    
    [Fact]
    public void HtmlSingleHoldingTest()
    {
        // Arrange
        var holdings = new List<Holding>();
        holdings.Add(new Holding
        {
            Fund = "Test",
            Ticker = "TST",
            Company = "Test Company",
            Cusip = "123456789",
            Shares = 100,
            MarketValueUsd = 1000,
            WeightPercentage = 10,
            Date = DateTime.Now
        });
        
        // Act
        var output = _formatService.FormatHTMLHoldingsTable(holdings);
        
        // Assert
        Assert.Equal("<table><thead><tr><th>Ticker</th><th>Shares</th></tr></thead><tbody><tr><td>TST</td><td>100</td></tr></tbody></table>", output);
    }
    
    [Fact]
    public void HtmlMultipleHoldingTest()
    {
        // Arrange
        var holdings = new List<Holding>();
        holdings.Add(new Holding
        {
            Fund = "Test",
            Ticker = "TST",
            Company = "Test Company",
            Cusip = "123456789",
            Shares = 100,
            MarketValueUsd = 1000,
            WeightPercentage = 10,
            Date = DateTime.Now
        });
        holdings.Add(new Holding
        {
            Fund = "Test",
            Ticker = "TST2",
            Company = "Test Company",
            Cusip = "123456789",
            Shares = 100,
            MarketValueUsd = 1000,
            WeightPercentage = 10,
            Date = DateTime.Now
        });
        
        // Act
        var output = _formatService.FormatHTMLHoldingsTable(holdings);
        
        // Assert
        Assert.Equal("<table><thead><tr><th>Ticker</th><th>Shares</th></tr></thead><tbody><tr><td>TST</td><td>100</td></tr><tr><td>TST2</td><td>100</td></tr></tbody></table>", output);
    }
    
    [Fact]
    public void FormatTableEmptyStringTest()
    {
        // Arrange
        var holdings = new List<Holding>();
        
        // Act
        var output = _formatService.FormatHoldingsTable(holdings);
        var actual = output.ToString();
        
        // Assert
        Assert.Equal(HoldingTableHeader, actual);
    }
    
    [Fact]
    public void FormatTableSingleHoldingTest()
    {
        // Arrange
        var holdings = new List<Holding>();
        holdings.Add(new Holding
        {
            Fund = "Test",
            Ticker = "TST",
            Company = "Test Company",
            Cusip = "123456789",
            Shares = 100,
            MarketValueUsd = 1000,
            WeightPercentage = 10,
            Date = new DateTime(2024, 4, 16)
        });
        var expected = HoldingTableHeader + $"16.04.2024 | Test                                | Test Company                             | TST        | 123456789  |        100 |            1000 |         10\n";
        
        // Act
        var output = _formatService.FormatHoldingsTable(holdings);
        var actual = output.ToString();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void FormatTableMultipleHoldingTest()
    {
        // Arrange
        var holdings = new List<Holding>();
        holdings.Add(new Holding
        {
            Fund = "Test",
            Ticker = "TST",
            Company = "Test Company",
            Cusip = "123456789",
            Shares = 100,
            MarketValueUsd = 1000,
            WeightPercentage = 10,
            Date = new DateTime(2024, 4, 16)
        });
        holdings.Add(new Holding
        {
            Fund = "Test",
            Ticker = "TST2",
            Company = "Test Company",
            Cusip = "123456789",
            Shares = 100,
            MarketValueUsd = 1000,
            WeightPercentage = 10,
            Date = new DateTime(2024, 4, 16)
        });
        var expected = HoldingTableHeader + $"16.04.2024 | Test                                | Test Company                             | TST        | 123456789  |        100 |            1000 |         10\n";
        expected += $"16.04.2024 | Test                                | Test Company                             | TST2       | 123456789  |        100 |            1000 |         10\n";
        
        // Act
        var output = _formatService.FormatHoldingsTable(holdings);
        var actual = output.ToString();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void ReducedFormatTableEmptyStringTest()
    {
        // Arrange
        var holdings = new List<Holding>();
        
        // Act
        var output = _formatService.FormatReducedHoldingsTable(holdings);
        var actual = output.ToString();
        
        // Assert
        Assert.Equal(ReducedHoldingTableHeader, actual);
    }
    
    [Fact]
    public void ReducedFormatTableSingleHoldingTest()
    {
        // Arrange
        var holdings = new List<Holding>();
        holdings.Add(new Holding
        {
            Fund = "Test",
            Ticker = "TST",
            Company = "Test Company",
            Cusip = "123456789",
            Shares = 100,
            MarketValueUsd = 1000,
            WeightPercentage = 10,
            Date = new DateTime(2024, 4, 16)
        });
        var expected = ReducedHoldingTableHeader + $"TST | 100\n";
        
        // Act
        var output = _formatService.FormatReducedHoldingsTable(holdings);
        var actual = output.ToString();
        
        // Assert
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public void ReducedFormatTableMultipleHoldingTest()
    {
        // Arrange
        var holdings = new List<Holding>();
        holdings.Add(new Holding
        {
            Fund = "Test",
            Ticker = "TST",
            Company = "Test Company",
            Cusip = "123456789",
            Shares = 100,
            MarketValueUsd = 1000,
            WeightPercentage = 10,
            Date = new DateTime(2024, 4, 16)
        });
        holdings.Add(new Holding
        {
            Fund = "Test",
            Ticker = "TST2",
            Company = "Test Company",
            Cusip = "123456789",
            Shares = 100,
            MarketValueUsd = 1000,
            WeightPercentage = 10,
            Date = new DateTime(2024, 4, 16)
        });
        var expected = ReducedHoldingTableHeader + $"TST | 100\n";
        expected += $"TST2 | 100\n";
        
        // Act
        var output = _formatService.FormatReducedHoldingsTable(holdings);
        var actual = output.ToString();
        
        // Assert
        Assert.Equal(expected, actual);
    }
}