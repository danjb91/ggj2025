using TMPro;
using UnityEngine;

public class StockPanelItem : MonoBehaviour
{
    public StockSimulation simulation;
    public StockInstance stock;
    TMP_Text ticker;
    TMP_Text heldText;
    public void BuyStock()
    {
        simulation.BuyStock(stock.Name);
    }
    public void SellStock()
    {
        simulation.SellStock(stock.Name);
    }

    private void Start()
    {
        ticker = transform.Find("Ticker").GetComponent<TMP_Text>();
        heldText = transform.Find("HeldAmount").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        if (ticker == null || stock == null)
        {
            return;
        }
        ticker.text = $"<color={(stock.Delta > 0 ? "green" : "red")}>{stock.Name} - {stock.Price.ToString("0.0")} ({stock.Delta.ToString("0.00")})</color>";
        heldText.text = $"Held: {simulation.GetAmountInPortfolio(stock.Name)}";
    }
}
