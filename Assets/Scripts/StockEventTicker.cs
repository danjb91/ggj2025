using UnityEngine;

public class StockEventTicker : MonoBehaviour
{
    StockChaosSim chaosSim;
    StockMessageManager messageManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chaosSim = GetComponentInParent<StockChaosSim>();
        chaosSim.OnStockEvent += OnStockEvent;
        
        messageManager = GetComponent<StockMessageManager>();
    }

    void OnStockEvent(object sender, StockEvent stockEvent)
    {
        var msg = messageManager.RandomMessage(stockEvent.EventType, stockEvent.Stock.Name);

        Debug.Log($"OnStockEvent [{stockEvent.EventType}] msg = " + msg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
