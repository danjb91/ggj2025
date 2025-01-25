public class Stock
{
    public string name { get; set; }
    public float price { get; set; }
    public float upperBound { get; set; }
    public float lowerBound { get; set; }
    // How much the stock bound is affected by sales or purchases
    public int totalShares { get; set; }
    // How much the stock floor and ceiling can change per tick
    public float volatility { get; set; }

    public Stock() => (name, price, upperBound, lowerBound, totalShares, volatility) = ("", 0, 0, 0, 100, 1f);
}
