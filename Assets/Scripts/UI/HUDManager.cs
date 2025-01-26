using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    Canvas canvas;
    GameObject gameUI;
    GameObject mainMenuUI;
    TMP_Text p1Portfolio;
    TMP_Text p2Portfolio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        gameUI = transform.Find("Game").gameObject;
        mainMenuUI = transform.Find("MainMenu").gameObject;
        p1Portfolio = gameUI.transform.Find("Player1Portfolio").GetComponent<TMP_Text>();
        p2Portfolio = gameUI.transform.Find("Player2Portfolio").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        p1Portfolio.text = $"{GameManager.Instance.stockSim.GetTotalPortfolio(1)}";
        p2Portfolio.text = $"{GameManager.Instance.stockSim.GetTotalPortfolio(2)}";
    }
}
