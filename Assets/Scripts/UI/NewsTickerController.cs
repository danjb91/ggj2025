using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class NewsTickerController : MonoBehaviour
{
    public NewsTickerItemController newsTickerItemPrefab;
    public float itemDuration = 2.0f;

    float width;
    float pixelsPerSecond;
    List<NewsTickerItemController> currentItems = new();

    StockChaosSim chaosSim;
    StockMessageManager messageManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        chaosSim = GetComponentInParent<StockChaosSim>();
        chaosSim.OnStockEvent += OnStockEvent;
        
        messageManager = GetComponentInParent<StockMessageManager>();
    }

    void OnStockEvent(object sender, StockEvent stockEvent)
    {
        // if (!CanSpawnNewItem()))
        // {
        //     Debug.LogError("News ticker is already scrolling an item!!!!!!!!!");
        //     return;
        // }

        var msg = messageManager.RandomMessage(stockEvent.EventType, stockEvent.Stock.Name);

        Debug.Log($"OnStockEvent [{stockEvent.EventType}] msg = " + msg);


        var newItem = Instantiate<NewsTickerItemController>(newsTickerItemPrefab, this.gameObject.transform);
        newItem.Init(msg);
        newItem.Text.autoSizeTextContainer = true;
        newItem.Text.ForceMeshUpdate();

        var lastItem = currentItems.LastOrDefault();

        if (lastItem != null)  
        {
            // this doesnt work
            newItem.Rect.position = lastItem.Rect.position + Vector3.right * (lastItem.Text.preferredWidth + (5 * lastItem.Text.text.Length));
        }

        currentItems.Add(newItem);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWidth();

        foreach (var item in currentItems)
            UpdateItem(item);

    }

    void UpdateItem(NewsTickerItemController item)
    {
        item.Rect.position += Vector3.left * pixelsPerSecond * Time.deltaTime;
    }

    // bool CanSpawnNewItem()
    // {
    //     if (currentItem == null)
    //     {

    //     }
    // }

    void UpdateWidth()
    {
        width = GetComponent<RectTransform>().rect.width;
        pixelsPerSecond = width / itemDuration;
    }
}
