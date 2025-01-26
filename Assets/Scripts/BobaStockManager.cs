using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

struct BobaStockToSpawn
{
    public string stockName;
    public int shares;
    public Material material;
}

public class BobaStockManager : MonoBehaviour
{
    public BoxCollider spawner;
    public GameObject bobaPrefab;
    public float spawnRate = 0.5f;
    private float lastUpdate = 0f;
    Dictionary<string, int> sharesInPlay = new Dictionary<string, int>();
    Stack<BobaStockToSpawn> bobaStockToSpawn = new Stack<BobaStockToSpawn>();
    double lastStockCheck = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.bobaStockManager = this;
        foreach (StockInstance stock in GameManager.Instance.stockSim.CurrentStocks)
        {
            int splitAmount = stock.TotalShares / 10;
            for (int i = 0; i < stock.TotalShares; i+= splitAmount)
            {
                bobaStockToSpawn.Push(new BobaStockToSpawn { stockName = stock.Name, shares = splitAmount, material = stock.Material });
            }
        }
        GameManager.Instance.stockSim.StockAdded += OnStockAdded;
        GameManager.Instance.stockSim.StockCrashed += OnStockRemoved;
    }

    private void OnStockAdded(object sender, StockInstance stock)
    {
        int splitAmount = stock.TotalShares / 10;
        for (int i = 0; i < stock.TotalShares; i += splitAmount)
        {
            bobaStockToSpawn.Push(new BobaStockToSpawn { stockName = stock.Name, shares = splitAmount, material = stock.Material });
        }
    }

    private void OnStockRemoved(object sender, StockInstance e) {
        var allBoba = FindObjectsByType<BobaEntity>(FindObjectsSortMode.None);
        sharesInPlay.Remove(e.Name);
        foreach (var boba in allBoba)
        {
            if (boba.getStock() == e.Name)
            {
                Destroy(boba.gameObject);
            }
        }
    }

    public void AddStockToSpawn(string stockName, int shares, Material material)
    {
        bobaStockToSpawn.Push(new BobaStockToSpawn { stockName = stockName, shares = shares, material = material });
    }

    public void RemoveBobaFromPlay(BobaEntity entity)
    {
        if (sharesInPlay.ContainsKey(entity.getStock()))
        {
            sharesInPlay[entity.getStock()] -= (int)entity.getShares();
        }
        Destroy(entity.gameObject);
    }

    void CheckStockAmounts()
    {
        foreach (var stockCounts in sharesInPlay)
        {
            var stock = GameManager.Instance.stockSim.GetStock(stockCounts.Key);
            if (stock == null)
            {
                continue;
            }
            int leftToSpawn = bobaStockToSpawn.Sum(b => b.stockName == stock.Name ? b.shares : 0);
            if (stockCounts.Value + leftToSpawn < stock.TotalShares)
            {
                int diff = stock.TotalShares - stockCounts.Value;
                int splitAmount = stock.TotalShares / 10;
                int i = 0;
                while (i < diff)
                {
                    int shares = Mathf.Min(splitAmount, diff - i);
                    bobaStockToSpawn.Push(new BobaStockToSpawn { stockName = stock.Name, shares = shares, material = stock.Material });
                    i += shares;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastUpdate < spawnRate)
        {
            return;
        }
        lastUpdate = Time.time;
        if (bobaStockToSpawn.Count > 0)
        {
            var stock = bobaStockToSpawn.Pop();
            var randomPos = new Vector3(spawner.transform.position.x, Random.Range(spawner.bounds.min.y, spawner.bounds.max.y), Random.Range(spawner.bounds.min.z, spawner.bounds.max.z));
            var boba = Instantiate(bobaPrefab, randomPos, Quaternion.identity);
            var bobaEntity = boba.GetComponent<BobaEntity>();
            bobaEntity.setStock(stock.stockName);
            bobaEntity.setShares(stock.shares);
            var bobaRenderer = boba.GetComponent<MeshRenderer>();
            bobaRenderer.material = stock.material;
            bobaEntity.SetupScale();
            if (sharesInPlay.ContainsKey(stock.stockName))
            {
                sharesInPlay[stock.stockName] += stock.shares;
            }
            else
            {
                sharesInPlay.Add(stock.stockName, stock.shares);
            }
        }
        if (Time.time - lastStockCheck > 2)
        {
            CheckStockAmounts();
            lastStockCheck = Time.time;
        }
    }
}
