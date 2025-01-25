using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;

public struct StockTicker
{
    public TMP_Text text;
    public Button buyButton;
    public Button sellButton;
    public TMP_Text heldAmount;
    public StockPanelItem item;
}

public class StockSimulation : MonoBehaviour
{
    public Stock[] stocks =
    {
        new Stock { name = "AAPL", price = 150.0f, upperBound = 4.0f, lowerBound = -1.0f, totalShares = 20, volatility = 1.0f },
        new Stock { name = "GOOGL", price = 250.0f, upperBound = 5.0f, lowerBound = -1.0f, totalShares = 20, volatility = 1.0f },
        new Stock { name = "AMZN", price = 350.0f, upperBound = 3.0f, lowerBound = -1.0f, totalShares = 20, volatility = 1.0f },
        new Stock { name = "MSFT", price = 30.0f, upperBound = 4.0f, lowerBound = -1.0f, totalShares = 20, volatility = 1.0f },
        new Stock { name = "TSLA", price = 70.0f, upperBound = 3.0f, lowerBound = -1.0f, totalShares = 20, volatility = 1.0f }
    };
    public Canvas canvas;
    public GameObject stockPanel;
    public GameObject textPrefab;
    public TMP_Text currentMoney;
    public TMP_Text portfolioValue;
    public Dictionary<string, StockTicker> stockPrefabs = new Dictionary<string, StockTicker>();
    public Dictionary<string, int> portfolio = new Dictionary<string, int>();
    private double lastUpdate = 0;

    public double money = 1000.0;
    public double refreshRate = 1.0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        foreach (Stock stock in stocks)
        {
            var stockPrefab = Instantiate(textPrefab, stockPanel.transform);
            var ticker = new StockTicker
            {
                text = stockPrefab.transform.Find("Ticker").GetComponent<TMP_Text>(),
                buyButton = stockPrefab.transform.Find("BuyButton").GetComponent<Button>(),
                sellButton = stockPrefab.transform.Find("SellButton").GetComponent<Button>(),
                heldAmount = stockPrefab.transform.Find("HeldAmount").GetComponent<TMP_Text>(),
                item = stockPrefab.GetComponent<StockPanelItem>()
            };
            ticker.item.stock = stock;
            stockPrefabs.Add(stock.name, ticker);
        }
    }

    public void SetStockVolatiliy(string stock, float upperBound, float lowerBound)
    {
        stocks[Array.FindIndex(stocks, s => s.name == stock)].upperBound = upperBound;
        stocks[Array.FindIndex(stocks, s => s.name == stock)].lowerBound = lowerBound;
    }

    public void SetStockPrice(string stock, float price)
    {
        if (price < 0)
        {
            price = 0;
        }
        stocks[Array.FindIndex(stocks, s => s.name == stock)].price = price;
    }

    public Stock GetStock(string stock)
    {
        return stocks[Array.FindIndex(stocks, s => s.name == stock)];
    }

    public Stock GetRandomStock()
    {
        return stocks[UnityEngine.Random.Range(0, stocks.Length)];
    }

    public void BuyStock(string stock)
    {
        var currentStock = GetStock(stock);
        if (money >= currentStock.price)
        {
            var proportion = (1f / currentStock.totalShares);
            var bonus = 1 + proportion * (1f + UnityEngine.Random.value * currentStock.volatility);
            currentStock.price *= bonus;
            currentStock.upperBound *= 1f - proportion;
            Debug.Log($"Buy Bonus: {bonus}");
            money -= currentStock.price;
            if (portfolio.ContainsKey(stock))
            {
                portfolio[stock]++;
            }
            else
            {
                portfolio.Add(stock, 1);
            }
        }
    }
    public void SellStock(string stock)
    {
        var currentStock = GetStock(stock);
        if (portfolio.ContainsKey(stock))
        {
            money += currentStock.price;
            var proportion = (1f / currentStock.totalShares);
            var penalty = 1 - proportion * (1f + UnityEngine.Random.value * currentStock.volatility);
            currentStock.lowerBound *= 1f + proportion;
            currentStock.price *= penalty;
            Debug.Log($"Sell Penalty: {penalty}");
            portfolio[stock]--;
            if (portfolio[stock] == 0)
            {
                portfolio.Remove(stock);
            }
        }
    }

    void UpdateStatusText()
    {
        currentMoney.text = $"Money: {(int)money}";
        double currentPortfolio = 0;
        foreach (var stockInPortfolio in portfolio)
        {
            var currentStock = stocks[Array.FindIndex(stocks, s => s.name == stockInPortfolio.Key)];
            currentPortfolio += currentStock.price * (double)stockInPortfolio.Value;
        }
        portfolioValue.text = $"Portfolio Value: {(int)currentPortfolio}";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStatusText();
        if (Time.timeAsDouble - lastUpdate < refreshRate && lastUpdate > 0)
        {
            return;
        }

        lastUpdate = Time.timeAsDouble;
        foreach (Stock stock in stocks)
        {
            var stockDelta = UnityEngine.Random.Range(stock.lowerBound, stock.upperBound);
            stock.price = Math.Max(0f, stock.price + stockDelta);
            stock.upperBound += UnityEngine.Random.Range(-(stock.volatility / 2f), (stock.volatility / 2f));
            stock.lowerBound += UnityEngine.Random.Range(-(stock.volatility / 2f), (stock.volatility / 2f));
            stock.volatility = Math.Max(1f, stock.volatility - UnityEngine.Random.value);
            try
            {
                var prefabs = stockPrefabs[stock.name];
                prefabs.text.text = $"<color=\"{(stockDelta > 0 ? "green" : "red")}\">{stock.name}</color>: {stock.price.ToString("0.0")} ({stockDelta.ToString("0.0")})";
                prefabs.heldAmount.text = portfolio.ContainsKey(stock.name) ? portfolio[stock.name].ToString() : "0";
            }
            catch (KeyNotFoundException)
            {
            }
        }
    }
}
