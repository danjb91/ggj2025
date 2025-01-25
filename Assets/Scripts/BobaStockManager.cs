using System.Collections.Generic;
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
    Stack<BobaStockToSpawn> bobaStockToSpawn = new Stack<BobaStockToSpawn>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.bobaStockManager = this;
        foreach (StockInstance stock in GameManager.Instance.stockSim.CurrentStocks)
        {
            for (int i = 0; i < stock.TotalShares; i+=10)
            {
                bobaStockToSpawn.Push(new BobaStockToSpawn { stockName = stock.Name, shares = 10, material = stock.Material });
            }
        }
        GameManager.Instance.stockSim.StockAdded += OnStockAdded;
        GameManager.Instance.stockSim.StockCrashed += OnStockRemoved;
    }

    private void OnStockAdded(object sender, StockInstance stock)
    {
        for (int i = 0; i < stock.TotalShares; i += 10)
        {
            bobaStockToSpawn.Push(new BobaStockToSpawn { stockName = stock.Name, shares = 10, material = stock.Material });
        }
    }

    private void OnStockRemoved(object sender, StockInstance e) {
        var allBoba = FindObjectsByType<BobaEntity>(FindObjectsSortMode.None);
        foreach (var boba in allBoba)
        {
            if (boba.getStock() == e.Name)
            {
                Destroy(boba.gameObject);
            }
        }
    }

    public void AddStockToSpawn(string stockName, int shares)
    {
        bobaStockToSpawn.Push(new BobaStockToSpawn { stockName = stockName, shares = shares });
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
            var randomPos = new Vector3(0f, Random.Range(spawner.bounds.min.y, spawner.bounds.max.y), Random.Range(spawner.bounds.min.z, spawner.bounds.max.z));
            var boba = Instantiate(bobaPrefab, randomPos, Quaternion.identity);
            var bobaEntity = boba.GetComponent<BobaEntity>();
            bobaEntity.setStock(stock.stockName);
            bobaEntity.setShares(stock.shares);
            var bobaRenderer = boba.GetComponent<MeshRenderer>();
            bobaRenderer.material = stock.material;
        }
    }
}
