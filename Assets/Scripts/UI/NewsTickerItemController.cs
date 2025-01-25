using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsTickerItemController : MonoBehaviour
{
    public RectTransform Rect {get; private set;}
    public TMP_Text Text {get; private set;}

    public float Width => Rect.rect.width;
    public float XPosition => Rect.anchoredPosition.x; 

    public void Init(string msg)
    {
        Text = GetComponent<TMP_Text>();
        Rect = GetComponent<RectTransform>();

        Text.text = msg;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
