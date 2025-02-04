using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;

public class StockSimulation : MonoBehaviour
{
    public List<Stonk> StonkPool;
    public List<StockInstance> CurrentStocks = new List<StockInstance>();
    public Dictionary<string, int>[] portfolios = { new Dictionary<string, int>(), new Dictionary<string, int>() };
    private double lastUpdate = 0;
    public event EventHandler<StockInstance> StockAdded;
    public event EventHandler<StockInstance> StockCrashed;

    public List<double> currentMoney = new List<double>{ 0.0, 0.0 };
    public double refreshRate = 1.0;

    public delegate void StockSimulationTickHandler();
    public delegate void StockSimulationResetHandler();

    public event StockSimulationTickHandler OnSimulationTick;
    public event StockSimulationResetHandler OnSimulationReset;

    public void SetStockVolatiliy(string stock, float upperBound, float lowerBound)
    {
        var currentStock = GetStock(stock);
        currentStock.UpperBound = upperBound;
        currentStock.LowerBound = lowerBound;
    }

    public void SetStockPrice(string stock, float price)
    {
        if (price < 0)
        {
            price = 0;
        }
        var currentStock = GetStock(stock);
        currentStock.Price = price;
    }

    public StockInstance GetStock(string stock)
    {
        return CurrentStocks.Find(s => s.Name == stock);
    }

    public double GetMoney(int player = 1)
    {
        return currentMoney[player - 1];
    }

    public StockInstance GetRandomStock()
    {
        return CurrentStocks[UnityEngine.Random.Range(0, CurrentStocks.Count)];
    }

    public int GetAmountInPortfolio(string stock, int player = 1)
    {
        if (portfolios[player - 1].ContainsKey(stock))
        {
            return portfolios[player - 1][stock];
        }
        return 0;
    }

    public int GetTotalPortfolio(int player = 1)
    {
        int value = 0;
        foreach (var stock in portfolios[player - 1])
        {
            var currentStock = GetStock(stock.Key);
            value += (int)(currentStock.Price * stock.Value);
        }
        return value;
    }

    public void BuyStock(string stock, int player = 1)
    {
        var currentStock = GetStock(stock);
        if (currentStock == null)
        {
            return;
        }
        var proportion = (1f / currentStock.TotalShares);
        var bonus = 1 + proportion * (1f + UnityEngine.Random.value * currentStock.Volatility);
        currentStock.Price *= bonus;
        currentStock.UpperBound *= 1f - proportion;
        Debug.Log($"Buy Bonus: {bonus}");
        currentMoney[player - 1] = Math.Max(0, currentMoney[player - 1] - currentStock.Price);
        if (portfolios[player - 1].ContainsKey(stock))
        {
            portfolios[player - 1][stock]++;
        }
        else
        {
            portfolios[player - 1].Add(stock, 1);
        }
    }
    public void SellStock(string stock, int player = 1)
    {
        var currentStock = GetStock(stock);
        if (currentStock == null)
        {
            return;
        }
        if (portfolios[player - 1].ContainsKey(stock))
        {
            currentMoney[player - 1] += currentStock.Price;
            var proportion = (1f / currentStock.TotalShares);
            var penalty = 1 - proportion * (1f + UnityEngine.Random.value * currentStock.Volatility);
            currentStock.LowerBound *= 1f + proportion;
            currentStock.Price *= penalty;
            Debug.Log($"Sell Penalty: {penalty}");
            portfolios[player - 1][stock]--;
            if (portfolios[player - 1][stock] == 0)
            {
                portfolios[player - 1].Remove(stock);
            }
        }
    }

    public void RemoveStock(string stock, int player = 1)
    {
        var currentStock = GetStock(stock);
        bool result = CurrentStocks.Remove(currentStock);
        Debug.Log($"Removing stock {stock}: {result}");
        foreach (var portfolio in portfolios)
        {
            if (portfolio.ContainsKey(stock))
            {
                portfolio.Remove(stock);
            }
        }
        StockCrashed?.Invoke(this, currentStock);
    }

    public void AddNewStock(string doNotPick = "")
    {
        Stonk selectedStock;
        foreach (Stonk stock in StonkPool)
        {
            if (!CurrentStocks.Exists(s => s.Name == stock.name) && stock.name != doNotPick)
            {
                selectedStock = stock;
                var newStock = new StockInstance(selectedStock);
                CurrentStocks.Add(newStock);
                StockAdded?.Invoke(this, newStock);
                Debug.Log($"Added stock {newStock.Name}");
                break;
            }
        }
    }

    public void TickSimulation()
    {
        lastUpdate = Time.timeAsDouble;
        foreach (StockInstance stock in CurrentStocks)
        {
            var stockDelta = UnityEngine.Random.Range(stock.LowerBound, stock.UpperBound);
            stock.Delta = stockDelta;
            stock.Price = Math.Max(0f, stock.Price + stockDelta);
            stock.UpperBound = Math.Max(stock.LowerBound, stock.UpperBound + UnityEngine.Random.Range(-(stock.Volatility / 2f), (stock.Volatility / 2f)));
            stock.LowerBound = Math.Min(stock.UpperBound, stock.LowerBound + UnityEngine.Random.Range(-(stock.Volatility / 2f), (stock.Volatility / 2f)));
            stock.Volatility = Math.Max(1f, stock.Volatility - UnityEngine.Random.value);
            int prevStock = stock.TotalShares;
            stock.TotalShares = Math.Max(stock.TotalShares, Mathf.CeilToInt(stock.Price / 100f) * 50 + 50);
            if (stock.TotalShares != prevStock) 
            {
                Debug.Log($"Stock {stock.Name} has changed total shares from {prevStock} to {stock.TotalShares}");
            }
        }

        if (OnSimulationTick != null)
            OnSimulationTick();

        var stocksToRemove = CurrentStocks.Where(s => s.Price <= 0).ToList();
        foreach (var stock in stocksToRemove)
        {
            RemoveStock(stock.Name);
        }
        if (stocksToRemove.Count > 0)
        {
            for (int i = 0; i < stocksToRemove.Count; i++)
            {
                AddNewStock(stocksToRemove[i].Name);
            }
        }
    }

    private void Start()
    {
        GameManager.Instance.stockSim = this;
        int toCreate = 5;
        while (toCreate > 0)
        {
            AddNewStock();
            toCreate--;
        }
    }

    public void ResetGame()
    {
        var stocksToRemove = CurrentStocks.ToList();
        foreach (var stock in stocksToRemove)
        {
            RemoveStock(stock.Name);
        }

        Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeAsDouble - lastUpdate < refreshRate && lastUpdate > 0)
        {
            return;
        }

        TickSimulation();
    }
}
