public class Stock
{
    public string name { get; set; }
    public float price { get; set; }
    public float upperBound { get; set; }
    public float lowerBound { get; set; }

    public Stock() => (name, price, upperBound, lowerBound) = ("", 0, 0, 0);
}
