using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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
        new Stock { name = "AAPL", price = 150.0f, upperBound = 4.0f, lowerBound = -2.0f },
        new Stock { name = "GOOGL", price = 250.0f, upperBound = 5.0f, lowerBound = -3.0f },
        new Stock { name = "AMZN", price = 350.0f, upperBound = 3.0f, lowerBound = -3.0f },
        new Stock { name = "MSFT", price = 30.0f, upperBound = 4.0f, lowerBound = -2.0f },
        new Stock { name = "TSLA", price = 70.0f, upperBound = 3.0f, lowerBound = -10.0f }
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

    public void BuyStock(string stock)
    {
        if (money >= stocks[Array.FindIndex(stocks, s => s.name == stock)].price)
        {
            money -= stocks[Array.FindIndex(stocks, s => s.name == stock)].price;
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
        if (portfolio.ContainsKey(stock))
        {
            money += stocks[Array.FindIndex(stocks, s => s.name == stock)].price;
            portfolio[stock]--;
            if (portfolio[stock] == 0)
            {
                portfolio.Remove(stock);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeAsDouble - lastUpdate < refreshRate && lastUpdate > 0)
        {
            return;
        }

        lastUpdate = Time.timeAsDouble;
        foreach (Stock stock in stocks)
        {
            stock.price = Math.Max(0f, stock.price + UnityEngine.Random.Range(stock.lowerBound, stock.upperBound));
            stock.upperBound += UnityEngine.Random.Range(-0.5f, 0.5f);
            stock.lowerBound += UnityEngine.Random.Range(-0.5f, 0.5f);
            try
            {
                var prefabs = stockPrefabs[stock.name];
                prefabs.text.text = $"{stock.name}: {stock.price.ToString("0.0")}";
                prefabs.heldAmount.text = portfolio.ContainsKey(stock.name) ? portfolio[stock.name].ToString() : "0";
            }
            catch (KeyNotFoundException)
            {
            }
        }

        currentMoney.text = $"Money: {(int)money}";
        double currentPortfolio = 0;
        foreach (var stockInPortfolio in portfolio)
        {
            var currentStock = stocks[Array.FindIndex(stocks, s => s.name == stockInPortfolio.Key)];
            currentPortfolio += currentStock.price * (double)stockInPortfolio.Value;
        }
        portfolioValue.text = $"Portfolio Value: {(int)currentPortfolio}";
    }
}
