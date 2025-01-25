using UnityEngine;

public class StockInstance
{
    public string Name { get; set; }
    public float Price { get; set; }
    public float UpperBound { get; set; }
    public float LowerBound { get; set; }
    // How much the stock bound is affected by sales or purchases
    public int TotalShares { get; set; }
    // How much has the stock last changed
    public float Delta { get; set; }

    // How much the stock floor and ceiling can change per tick
    public float Volatility { get; set; }

    public Color32 Color { get; set; }

    public StockInstance() => (Name, Price, UpperBound, LowerBound, TotalShares, Volatility, Color) = ("", 0, 0, 0, 100, 1f, new Color32());
    public StockInstance(Stonk stonk) => (Name, Price, UpperBound, LowerBound, TotalShares, Volatility, Color) = (stonk.name, stonk.Price, stonk.UpperBound, stonk.LowerBound, stonk.TotalShares, stonk.Volatility, stonk.Color);
}
