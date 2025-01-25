using UnityEngine;

public enum EventType
{
    SURGE,
    CRASH,
    DIP,
    PEAK,
    CONTROVERSY
}

public class StockChaosSim : MonoBehaviour
{
    StockSimulation simulation;
    double nextUpdate = 0;
    public double minRefreshRate = 5.0;
    public double refreshRate = 8.0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        simulation = GetComponent<StockSimulation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeAsDouble < nextUpdate)
        {
            return;
        }

        var stock = simulation.GetRandomStock();
        var eventType = (EventType)UnityEngine.Random.Range(0, 4);
        switch (eventType)
        {
            case EventType.SURGE:
            {
                var bonus = 1f + (UnityEngine.Random.value / 10f);
                stock.price *= bonus;
                Debug.Log($"{stock.name} raced up by {bonus}");
                break;
            }
            case EventType.CRASH:
            {
                var penalty = 1f - (UnityEngine.Random.value / 10f);
                stock.price *= penalty;
                Debug.Log($"{stock.name} crashed down by {penalty}");
                break;
            }
            case EventType.DIP:
            {
                var dip = stock.volatility * UnityEngine.Random.value * 10f;
                stock.lowerBound -= dip;
                Debug.Log($"{stock.name} dipped by {dip}");
                break;
            }
            case EventType.PEAK:
            {
                var peak = stock.volatility * UnityEngine.Random.value * 10f;
                stock.upperBound += peak;
                Debug.Log($"{stock.name} is peaking by {peak}");
                break;
            }
            case EventType.CONTROVERSY:
            {
                stock.volatility *= Random.Range(5f, 10f);
                Debug.Log($"{stock.name} is now controversial");
                break;
            }
        }

        nextUpdate = Time.timeAsDouble + refreshRate;
    }
}
