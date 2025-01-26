using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class StockMessageManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    Dictionary<EventType, List<string>> messages = new();

    void Start()
    {
        foreach (var x in Enum.GetValues(typeof(EventType)))
            LoadMessages((EventType)x);
    }

    void LoadMessages(EventType type)
    {
        var fname = $"stocks_{type.ToString().ToUpperInvariant()}";
        var res =  Resources.Load<TextAsset>(fname);

        if (res == null)
        {
            Debug.LogError("Failed to load "  + fname);
            return;
        }

        var txt = Regex.Replace(res.text, @"\r\n?|\n", "\n").Split("\n");

        messages.Add(type, new(txt));

        Debug.Log($"{fname}: {messages[type].Count} messages");
    }

    public string? RandomMessage(EventType type, string stockName, Color32 stockColor)
    {
        if (messages[type] == null)
            return null;

        string stockTag = $"<color=#{ColorUtility.ToHtmlStringRGB(stockColor)}>{stockName}</color>";

        var id = UnityEngine.Random.Range(0, (int)messages[type].Count);
        return messages[type][id].Replace("GGX", stockTag);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
