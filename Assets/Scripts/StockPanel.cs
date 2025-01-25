using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct StockTicker
{
    public TMP_Text text;
    public Button buyButton;
    public Button sellButton;
    public TMP_Text heldAmount;
    public StockPanelItem item;
    public GameObject gameObject;
}


public class StockPanel : MonoBehaviour
{
    StockSimulation simulation;
    public Canvas canvas;
    public GameObject stockContainer;
    public GameObject stockPrefab;
    public TMP_Text currentMoney;
    public TMP_Text portfolioValue;
    public Dictionary<string, StockTicker> stockPrefabs = new Dictionary<string, StockTicker>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        simulation = GetComponent<StockSimulation>();
        canvas = GetComponent<Canvas>();
        foreach (StockInstance stock in simulation.CurrentStocks)
        {
            AddStockToPanel(stock);
        }
        simulation.StockAdded += OnStockAdded;
        simulation.StockCrashed += OnStockCrashed;
    }

    void OnStockAdded(object sender, StockInstance stock)
    {
        AddStockToPanel(stock);
    }

    void OnStockCrashed(object sender, StockInstance stock)
    {
        RemoveStockFromPanel(stock);
    }

    void AddStockToPanel(StockInstance stock)
    {
        var stockInstance = Instantiate(stockPrefab, stockContainer.transform);
        var ticker = new StockTicker
        {
            text = stockInstance.transform.Find("Ticker").GetComponent<TMP_Text>(),
            buyButton = stockInstance.transform.Find("BuyButton").GetComponent<Button>(),
            sellButton = stockInstance.transform.Find("SellButton").GetComponent<Button>(),
            heldAmount = stockInstance.transform.Find("HeldAmount").GetComponent<TMP_Text>(),
            item = stockInstance.GetComponent<StockPanelItem>(),
            gameObject = stockInstance
        };
        stockInstance.GetComponent<StockPanelItem>().simulation = simulation;
        ticker.item.stock = stock;
        stockPrefabs.Add(stock.Name, ticker);
    }

    void RemoveStockFromPanel(StockInstance stock)
    {
        if (stockPrefabs.ContainsKey(stock.Name))
        {
            Destroy(stockPrefabs[stock.Name].gameObject);
            stockPrefabs.Remove(stock.Name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentMoney.text = $"Money: {(int)simulation.GetMoney()}";
        double currentPortfolio = simulation.GetTotalPortfolio();
        portfolioValue.text = $"Portfolio Value: {(int)currentPortfolio}";
    }
}
