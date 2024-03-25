namespace QualityProject.DAL.Models;

public class Holding
{
    public DateTime Date { get; set; }
    public string Fund { get; set; }
    public string Company { get; set; }
    public string Ticker { get; set; }
    public string Cusip { get; set; }
    public int Shares { get; set; }
    public decimal MarketValueUsd { get; set; }
    public decimal WeightPercentage { get; set; }


    public static Holding operator -(Holding newHolding, Holding oldHolding)
    {
        return new Holding
        {
            Date = newHolding.Date, 
            Fund = newHolding.Fund,
            Company = newHolding.Company,
            Ticker = newHolding.Ticker,
            Cusip = newHolding.Cusip,
            Shares = newHolding.Shares - oldHolding.Shares,
            MarketValueUsd = newHolding.MarketValueUsd - oldHolding.MarketValueUsd,
            WeightPercentage = newHolding.WeightPercentage - oldHolding.WeightPercentage
        };
    }

    public override string ToString()
    {
        return $"{Date.ToShortDateString(),-10} | {Fund,-35} | {Company,-40} | {Ticker,-10} | {Cusip,-10} | {Shares,10:N0} | {MarketValueUsd,15} | {WeightPercentage,10}";
    }
}