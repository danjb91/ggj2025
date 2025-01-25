using UnityEngine;

public class StockPanelItem : MonoBehaviour
{
    public StockSimulation simulation;
    public Stock stock;
    public void BuyStock()
    {
        simulation.BuyStock(stock.name);
    }
    public void SellStock()
    {
        simulation.SellStock(stock.name);
    }
}
