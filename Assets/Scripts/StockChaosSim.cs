using System;
using UnityEngine;

public enum EventType
{
    SURGE,
    CRASH,
    DIP,
    PEAK,
    CONTROVERSY
}

public class StockEvent
{
    public StockInstance Stock {get;set;}
    public EventType EventType {get;set;}
}

public class StockChaosSim : MonoBehaviour
{
    StockSimulation simulation;
    double nextUpdate = 0;
    public double minRefreshRate = 5.0;
    public double refreshRate = 8.0;
    public event EventHandler<StockEvent> OnStockEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        simulation = GetComponent<StockSimulation>();
    }

    void TickSimulation()
    {

        var stock = simulation.GetRandomStock();
        var eventType = (EventType)UnityEngine.Random.Range(0, 5);
        switch (eventType)
        {
            case EventType.SURGE:
                {
                    var bonus = 1f + (UnityEngine.Random.value / 10f);
                    stock.Price *= bonus;
                    Debug.Log($"{stock.Name} raced up by {bonus}");
                    break;
                }
            case EventType.CRASH:
                {
                    var penalty = 1f - (UnityEngine.Random.value / 10f);
                    stock.Price *= penalty;
                    Debug.Log($"{stock.Name} crashed down by {penalty}");
                    break;
                }
            case EventType.DIP:
                {
                    var dip = stock.Volatility * UnityEngine.Random.value * 10f;
                    stock.LowerBound -= dip;
                    Debug.Log($"{stock.Name} dipped by {dip}");
                    break;
                }
            case EventType.PEAK:
                {
                    var peak = stock.Volatility * UnityEngine.Random.value * 10f;
                    stock.UpperBound += peak;
                    Debug.Log($"{stock.Name} is peaking by {peak}");
                    break;
                }
            case EventType.CONTROVERSY:
                {
                    stock.Volatility *= UnityEngine.Random.Range(5f, 10f);
                    Debug.Log($"{stock.Name} is now controversial");
                    break;
                }
        }

        var evt = new StockEvent
        {
            Stock = stock,
            EventType = eventType
        };

        OnStockEvent?.Invoke(this, evt);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeAsDouble < nextUpdate)
        {
            return;
        }

        TickSimulation();

        nextUpdate = Time.timeAsDouble + refreshRate;
    }
}
